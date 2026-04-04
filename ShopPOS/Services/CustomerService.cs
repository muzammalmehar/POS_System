using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class CustomerService
    {
        public List<CustomerRecord> GetCustomers()
        {
            List<CustomerRecord> items = new List<CustomerRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureCustomerImageColumn(connection);
                EnsureCustomerPaymentsTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            c.customer_id,
                            c.customer_name,
                            c.phone,
                            c.address,
                            c.image_path,
                            c.opening_balance,
                            c.balance_type,
                            c.is_active,
                            IFNULL(s.total_purchase, 0.00) AS total_purchase,
                            IFNULL(s.total_due, 0.00) AS total_due,
                            IFNULL(cp.total_paid, 0.00) AS total_paid
                        FROM customers c
                        LEFT JOIN
                        (
                            SELECT
                                customer_id,
                                SUM(grand_total) AS total_purchase,
                                SUM(grand_total - paid_amount) AS total_due
                            FROM sale_header
                            WHERE customer_id IS NOT NULL
                              AND IFNULL(is_refunded, 0) = 0
                            GROUP BY customer_id
                        ) s ON s.customer_id = c.customer_id
                        LEFT JOIN
                        (
                            SELECT
                                customer_id,
                                SUM(amount) AS total_paid
                            FROM customer_payments
                            GROUP BY customer_id
                        ) cp ON cp.customer_id = c.customer_id
                        ORDER BY c.customer_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerRecord item = new CustomerRecord();
                            item.CustomerId = Convert.ToInt32(reader["customer_id"]);
                            item.CustomerName = Convert.ToString(reader["customer_name"]);
                            item.Phone = Convert.ToString(reader["phone"]);
                            item.Address = Convert.ToString(reader["address"]);
                            item.ImagePath = Convert.ToString(reader["image_path"]);
                            item.OpeningBalance = Convert.ToDecimal(reader["opening_balance"]);
                            item.BalanceType = Convert.ToString(reader["balance_type"]);
                            item.IsActive = Convert.ToBoolean(reader["is_active"]);
                            item.PurchaseAmount = Convert.ToDecimal(reader["total_purchase"]);
                            item.SaleDueAmount = Convert.ToDecimal(reader["total_due"]);
                            item.PaymentReceivedAmount = Convert.ToDecimal(reader["total_paid"]);
                            item.NetBalance = CalculateNetBalance(item);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<CustomerLedgerItem> GetCustomerLedger(int customerId)
        {
            List<CustomerLedgerItem> items = new List<CustomerLedgerItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureCustomerPaymentsTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            entry_date,
                            entry_type,
                            reference_no,
                            debit,
                            credit,
                            remarks
                        FROM
                        (
                            SELECT
                                c.created_at AS entry_date,
                                'Opening' AS entry_type,
                                CONCAT('CUS-', c.customer_id) AS reference_no,
                                CASE WHEN c.balance_type = 'Receivable' THEN c.opening_balance ELSE 0 END AS debit,
                                CASE WHEN c.balance_type = 'Payable' THEN c.opening_balance ELSE 0 END AS credit,
                                'Opening balance' AS remarks
                            FROM customers c
                            WHERE c.customer_id = @customerId

                            UNION ALL

                            SELECT
                                sh.sale_date AS entry_date,
                                'Sale' AS entry_type,
                                sh.sale_no AS reference_no,
                                sh.grand_total AS debit,
                                sh.paid_amount AS credit,
                                IFNULL(sh.remarks, 'Customer sale') AS remarks
                            FROM sale_header sh
                            WHERE sh.customer_id = @customerId
                              AND IFNULL(sh.is_refunded, 0) = 0

                            UNION ALL

                            SELECT
                                cp.payment_date AS entry_date,
                                'Payment' AS entry_type,
                                CONCAT('CP-', cp.customer_payment_id) AS reference_no,
                                0 AS debit,
                                cp.amount AS credit,
                                IFNULL(cp.remarks, 'Customer payment') AS remarks
                            FROM customer_payments cp
                            WHERE cp.customer_id = @customerId
                        ) ledger
                        ORDER BY entry_date DESC, reference_no DESC;";
                    command.Parameters.AddWithValue("@customerId", customerId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerLedgerItem item = new CustomerLedgerItem();
                            item.EntryDate = Convert.ToDateTime(reader["entry_date"]);
                            item.EntryType = Convert.ToString(reader["entry_type"]);
                            item.ReferenceNo = Convert.ToString(reader["reference_no"]);
                            item.Debit = Convert.ToDecimal(reader["debit"]);
                            item.Credit = Convert.ToDecimal(reader["credit"]);
                            item.Remarks = Convert.ToString(reader["remarks"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<CustomerSaleHistoryItem> GetCustomerSaleHistory(int customerId)
        {
            List<CustomerSaleHistoryItem> items = new List<CustomerSaleHistoryItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        sale_no,
                        sale_date,
                        grand_total,
                        paid_amount,
                        (grand_total - paid_amount) AS due_amount,
                        payment_method,
                        remarks
                    FROM sale_header
                    WHERE customer_id = @customerId
                      AND IFNULL(is_refunded, 0) = 0
                    ORDER BY sale_date DESC, sale_id DESC;";
                command.Parameters.AddWithValue("@customerId", customerId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerSaleHistoryItem item = new CustomerSaleHistoryItem();
                        item.SaleNo = Convert.ToString(reader["sale_no"]);
                        item.SaleDate = Convert.ToDateTime(reader["sale_date"]);
                        item.GrandTotal = Convert.ToDecimal(reader["grand_total"]);
                        item.PaidAmount = Convert.ToDecimal(reader["paid_amount"]);
                        item.DueAmount = Convert.ToDecimal(reader["due_amount"]);
                        item.PaymentMethod = Convert.ToString(reader["payment_method"]);
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public int SaveCustomer(CustomerRecord customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                throw new InvalidOperationException("Enter customer name.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                EnsureCustomerImageColumn(connection);

                int customerId;
                if (customer.CustomerId > 0)
                {
                    command.CommandText = @"
                        UPDATE customers
                        SET
                            customer_name = @name,
                            phone = @phone,
                            address = @address,
                            image_path = @imagePath,
                            opening_balance = @openingBalance,
                            balance_type = @balanceType,
                            is_active = @isActive
                        WHERE customer_id = @customerId;";
                    command.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    customerId = customer.CustomerId;
                }
                else
                {
                    command.CommandText = @"
                        INSERT INTO customers
                        (
                            customer_name, phone, address, image_path, opening_balance, balance_type, is_active
                        )
                        VALUES
                        (
                            @name, @phone, @address, @imagePath, @openingBalance, @balanceType, @isActive
                        );";
                    customerId = 0;
                }

                command.Parameters.AddWithValue("@name", customer.CustomerName.Trim());
                command.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(customer.Phone) ? (object)DBNull.Value : customer.Phone.Trim());
                command.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(customer.Address) ? (object)DBNull.Value : customer.Address.Trim());
                command.Parameters.AddWithValue("@imagePath", string.IsNullOrWhiteSpace(customer.ImagePath) ? (object)DBNull.Value : customer.ImagePath.Trim());
                command.Parameters.AddWithValue("@openingBalance", customer.OpeningBalance);
                command.Parameters.AddWithValue("@balanceType", string.IsNullOrWhiteSpace(customer.BalanceType) ? "Receivable" : customer.BalanceType);
                command.Parameters.AddWithValue("@isActive", customer.IsActive);
                command.ExecuteNonQuery();

                if (customer.CustomerId <= 0)
                {
                    customerId = Convert.ToInt32(command.LastInsertedId);
                }

                return customerId;
            }
        }

        public long SaveCustomerPayment(CustomerPaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.CustomerId <= 0)
            {
                throw new InvalidOperationException("Select a customer.");
            }

            if (request.WalletAccountId <= 0)
            {
                throw new InvalidOperationException("Select a wallet.");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidOperationException("Payment amount must be greater than zero.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureCustomerPaymentsTable(connection);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        long paymentId;
                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                INSERT INTO customer_payments
                                (
                                    customer_id, wallet_account_id, amount, payment_date, remarks, created_by
                                )
                                VALUES
                                (
                                    @customerId, @walletId, @amount, @paymentDate, @remarks, @createdBy
                                );
                                SELECT LAST_INSERT_ID();";
                            command.Parameters.AddWithValue("@customerId", request.CustomerId);
                            command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                            command.Parameters.AddWithValue("@amount", request.Amount);
                            command.Parameters.AddWithValue("@paymentDate", request.PaymentDate);
                            command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                            command.Parameters.AddWithValue("@createdBy", request.UserId);
                            paymentId = Convert.ToInt64(command.ExecuteScalar());
                        }

                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                                UPDATE wallet_accounts
                                SET current_balance = current_balance + @amount
                                WHERE wallet_account_id = @walletId;";
                            command.Parameters.AddWithValue("@amount", request.Amount);
                            command.Parameters.AddWithValue("@walletId", request.WalletAccountId);
                            command.ExecuteNonQuery();
                        }

                        AccountingService.PostCustomerReceipt(
                            connection,
                            transaction,
                            paymentId,
                            request.WalletAccountId,
                            request.Amount,
                            request.Remarks,
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
        }

        public CustomerPaymentReceipt GetCustomerPaymentReceipt(long customerPaymentId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureCustomerImageColumn(connection);
                EnsureCustomerPaymentsTable(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            cp.customer_payment_id,
                            c.customer_name,
                            cp.amount,
                            cp.payment_date,
                            wa.account_name,
                            cp.remarks,
                            u.full_name,
                            (
                                CASE WHEN c.balance_type = 'Receivable' THEN c.opening_balance ELSE -c.opening_balance END
                                + IFNULL((
                                    SELECT SUM(sh.grand_total - sh.paid_amount)
                                    FROM sale_header sh
                                    WHERE sh.customer_id = c.customer_id
                                      AND IFNULL(sh.is_refunded, 0) = 0
                                ), 0.00)
                                - IFNULL((
                                    SELECT SUM(cp2.amount)
                                    FROM customer_payments cp2
                                    WHERE cp2.customer_id = c.customer_id
                                ), 0.00)
                            ) AS remaining_receivable
                        FROM customer_payments cp
                        INNER JOIN customers c ON c.customer_id = cp.customer_id
                        INNER JOIN wallet_accounts wa ON wa.wallet_account_id = cp.wallet_account_id
                        INNER JOIN users u ON u.user_id = cp.created_by
                        WHERE cp.customer_payment_id = @customerPaymentId;";
                    command.Parameters.AddWithValue("@customerPaymentId", customerPaymentId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException("Customer payment receipt was not found.");
                        }

                        CustomerPaymentReceipt receipt = new CustomerPaymentReceipt();
                        receipt.CustomerPaymentId = Convert.ToInt64(reader["customer_payment_id"]);
                        receipt.ReceiptNo = string.Format("CPR-{0:000000}", receipt.CustomerPaymentId);
                        receipt.CustomerName = Convert.ToString(reader["customer_name"]);
                        receipt.Amount = Convert.ToDecimal(reader["amount"]);
                        receipt.PaymentDate = Convert.ToDateTime(reader["payment_date"]);
                        receipt.WalletName = Convert.ToString(reader["account_name"]);
                        receipt.Remarks = Convert.ToString(reader["remarks"]);
                        receipt.CreatedByName = Convert.ToString(reader["full_name"]);
                        receipt.RemainingReceivable = Convert.ToDecimal(reader["remaining_receivable"]);
                        return receipt;
                    }
                }
            }
        }

        private static decimal CalculateNetBalance(CustomerRecord item)
        {
            decimal openingEffect = item.BalanceType == "Receivable"
                ? item.OpeningBalance
                : -item.OpeningBalance;

            return openingEffect + item.SaleDueAmount - item.PaymentReceivedAmount;
        }

        private static void EnsureCustomerImageColumn(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'customers'
                      AND COLUMN_NAME = 'image_path';";

                bool exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                if (exists)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "ALTER TABLE customers ADD COLUMN image_path VARCHAR(255) NULL AFTER address;";
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureCustomerPaymentsTable(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS customer_payments
                    (
                        customer_payment_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        customer_id INT NOT NULL,
                        wallet_account_id INT NOT NULL,
                        amount DECIMAL(18,2) NOT NULL,
                        payment_date DATETIME NOT NULL,
                        remarks VARCHAR(255) NULL,
                        created_by INT NOT NULL,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        CONSTRAINT fk_customer_payment_customer
                            FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_customer_payment_wallet
                            FOREIGN KEY (wallet_account_id) REFERENCES wallet_accounts(wallet_account_id)
                            ON DELETE RESTRICT ON UPDATE CASCADE,
                        CONSTRAINT fk_customer_payment_user
                            FOREIGN KEY (created_by) REFERENCES users(user_id)
                            ON DELETE RESTRICT ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }
        }
    }
}
