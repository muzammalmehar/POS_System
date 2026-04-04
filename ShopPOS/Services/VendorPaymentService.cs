using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class VendorPaymentService
    {
        public List<VendorDueItem> GetVendorDues()
        {
            List<VendorDueItem> items = new List<VendorDueItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsurePaymentTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            s.supplier_id,
                            s.supplier_name,
                            s.preferred_visit_day,
                            s.payment_cycle,
                            s.next_payment_date,
                            (
                                CASE WHEN s.balance_type = 'Payable' THEN s.opening_balance ELSE 0 END
                                + IFNULL((
                                    SELECT SUM(ph.remaining_amount)
                                    FROM purchase_header ph
                                    WHERE ph.supplier_id = s.supplier_id
                                ), 0.00)
                                - IFNULL((
                                    SELECT SUM(sp.amount)
                                    FROM supplier_payments sp
                                    WHERE sp.supplier_id = s.supplier_id
                                ), 0.00)
                            ) AS outstanding_amount
                        FROM suppliers s
                        WHERE s.is_active = 1
                        ORDER BY s.supplier_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorDueItem item = new VendorDueItem();
                            item.SupplierId = Convert.ToInt32(reader["supplier_id"]);
                            item.SupplierName = Convert.ToString(reader["supplier_name"]);
                            item.VisitDay = Convert.ToString(reader["preferred_visit_day"]);
                            item.PaymentCycle = Convert.ToString(reader["payment_cycle"]);
                            item.NextPaymentDate = reader["next_payment_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["next_payment_date"]);
                            item.OutstandingAmount = Convert.ToDecimal(reader["outstanding_amount"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public long SaveVendorPayment(VendorPaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.SupplierId <= 0)
            {
                throw new InvalidOperationException("Select a vendor.");
            }

            if (request.WalletAccountId <= 0)
            {
                throw new InvalidOperationException("Select a payment wallet.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("Payment amount must be greater than zero.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsurePaymentTable(connection);
                    long paymentId;

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            INSERT INTO supplier_payments (
                                supplier_id, wallet_account_id, amount, payment_date, notes, created_by
                            )
                            VALUES (
                                @supplierId, @walletId, @amount, @paymentDate, @notes, @createdBy
                            );
                            SELECT LAST_INSERT_ID();";
                        command.Parameters.AddWithValue("@supplierId", request.SupplierId);
                        command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                        command.Parameters.AddWithValue("@amount", request.Amount);
                        command.Parameters.AddWithValue("@paymentDate", request.PaymentDate);
                        command.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(request.Notes) ? (object)DBNull.Value : request.Notes.Trim());
                        command.Parameters.AddWithValue("@createdBy", request.UserId);
                        paymentId = Convert.ToInt64(command.ExecuteScalar());
                    }

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            UPDATE wallet_accounts
                            SET current_balance = current_balance - @amount
                            WHERE wallet_account_id = @walletId;";
                        command.Parameters.AddWithValue("@amount", request.Amount);
                        command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                        command.ExecuteNonQuery();
                    }

                    if (request.NextPaymentDate.HasValue)
                    {
                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                UPDATE suppliers
                                SET next_payment_date = @nextPaymentDate
                                WHERE supplier_id = @supplierId;";
                            command.Parameters.AddWithValue("@nextPaymentDate", request.NextPaymentDate.Value.Date);
                            command.Parameters.AddWithValue("@supplierId", request.SupplierId);
                            command.ExecuteNonQuery();
                        }
                    }

                    AccountingService.PostVendorPayment(
                        connection,
                        transaction,
                        paymentId,
                        request.WalletAccountId,
                        request.Amount,
                        request.Notes,
                        request.UserId);

                    transaction.Commit();
                    return paymentId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public VendorPaymentReceipt GetVendorPaymentReceipt(long supplierPaymentId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsurePaymentTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            sp.supplier_payment_id,
                            s.supplier_name,
                            sp.amount,
                            sp.payment_date,
                            wa.account_name,
                            sp.notes,
                            u.full_name,
                            (
                                CASE WHEN s.balance_type = 'Payable' THEN s.opening_balance ELSE 0 END
                                + IFNULL((
                                    SELECT SUM(ph.remaining_amount)
                                    FROM purchase_header ph
                                    WHERE ph.supplier_id = s.supplier_id
                                ), 0.00)
                                - IFNULL((
                                    SELECT SUM(sp2.amount)
                                    FROM supplier_payments sp2
                                    WHERE sp2.supplier_id = s.supplier_id
                                ), 0.00)
                            ) AS remaining_balance
                        FROM supplier_payments sp
                        INNER JOIN suppliers s ON s.supplier_id = sp.supplier_id
                        INNER JOIN wallet_accounts wa ON wa.wallet_account_id = sp.wallet_account_id
                        INNER JOIN users u ON u.user_id = sp.created_by
                        WHERE sp.supplier_payment_id = @supplierPaymentId;";
                    command.Parameters.AddWithValue("@supplierPaymentId", supplierPaymentId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException("Vendor payment receipt was not found.");
                        }

                        VendorPaymentReceipt receipt = new VendorPaymentReceipt();
                        receipt.SupplierPaymentId = Convert.ToInt64(reader["supplier_payment_id"]);
                        receipt.ReceiptNo = string.Format("VPR-{0:000000}", receipt.SupplierPaymentId);
                        receipt.VendorName = Convert.ToString(reader["supplier_name"]);
                        receipt.Amount = Convert.ToDecimal(reader["amount"]);
                        receipt.PaymentDate = Convert.ToDateTime(reader["payment_date"]);
                        receipt.WalletName = Convert.ToString(reader["account_name"]);
                        receipt.Notes = Convert.ToString(reader["notes"]);
                        receipt.CreatedByName = Convert.ToString(reader["full_name"]);
                        receipt.RemainingBalance = Convert.ToDecimal(reader["remaining_balance"]);
                        return receipt;
                    }
                }
            }
        }

        private static void EnsurePaymentTable(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS supplier_payments (
                        supplier_payment_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        supplier_id INT NOT NULL,
                        wallet_account_id INT NOT NULL,
                        amount DECIMAL(18,2) NOT NULL,
                        payment_date DATETIME NOT NULL,
                        notes VARCHAR(255) NULL,
                        created_by INT NOT NULL,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        CONSTRAINT fk_supplier_payments_supplier
                            FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id)
                            ON DELETE RESTRICT ON UPDATE CASCADE,
                        CONSTRAINT fk_supplier_payments_wallet
                            FOREIGN KEY (wallet_account_id) REFERENCES wallet_accounts(wallet_account_id)
                            ON DELETE RESTRICT ON UPDATE CASCADE,
                        CONSTRAINT fk_supplier_payments_user
                            FOREIGN KEY (created_by) REFERENCES users(user_id)
                            ON DELETE RESTRICT ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }
        }
    }
}
