using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class VendorService
    {
        public List<VendorRecord> GetVendors()
        {
            List<VendorRecord> items = new List<VendorRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);
                ExpiryService.EnsureExpirySchema(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            suppliers.supplier_id,
                            suppliers.supplier_name,
                            suppliers.phone,
                            suppliers.address,
                            suppliers.opening_balance,
                            suppliers.balance_type,
                            suppliers.is_active,
                            suppliers.preferred_visit_day,
                            suppliers.payment_cycle,
                            suppliers.credit_days,
                            suppliers.next_payment_date,
                            suppliers.notes,
                            IFNULL(p.total_purchase, 0.00) AS total_purchase,
                            IFNULL(p.total_due, 0.00) AS total_due,
                            IFNULL(sp.total_paid, 0.00) AS total_paid,
                            IFNULL(er.pending_count, 0) AS expiry_pending_count,
                            IFNULL(er.returned_count, 0) AS expiry_returned_count,
                            IFNULL(er.burnt_count, 0) AS expiry_burnt_count
                        FROM suppliers
                        LEFT JOIN
                        (
                            SELECT
                                supplier_id,
                                SUM(grand_total) AS total_purchase,
                                SUM(remaining_amount) AS total_due
                            FROM purchase_header
                            GROUP BY supplier_id
                        ) p ON p.supplier_id = suppliers.supplier_id
                        LEFT JOIN
                        (
                            SELECT
                                supplier_id,
                                SUM(amount) AS total_paid
                            FROM supplier_payments
                            GROUP BY supplier_id
                        ) sp ON sp.supplier_id = suppliers.supplier_id
                        LEFT JOIN
                        (
                            SELECT
                                supplier_id,
                                SUM(CASE WHEN resolution_status = 'Pending' THEN 1 ELSE 0 END) AS pending_count,
                                SUM(CASE WHEN resolution_status = 'ReturnedToVendor' THEN 1 ELSE 0 END) AS returned_count,
                                SUM(CASE WHEN resolution_status = 'Burnt' THEN 1 ELSE 0 END) AS burnt_count
                            FROM expired_product_records
                            GROUP BY supplier_id
                        ) er ON er.supplier_id = suppliers.supplier_id
                        ORDER BY suppliers.supplier_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorRecord item = new VendorRecord();
                            item.SupplierId = Convert.ToInt32(reader["supplier_id"]);
                            item.SupplierName = Convert.ToString(reader["supplier_name"]);
                            item.Phone = Convert.ToString(reader["phone"]);
                            item.Address = Convert.ToString(reader["address"]);
                            item.OpeningBalance = Convert.ToDecimal(reader["opening_balance"]);
                            item.BalanceType = Convert.ToString(reader["balance_type"]);
                            item.IsActive = Convert.ToBoolean(reader["is_active"]);
                            item.PreferredVisitDay = Convert.ToString(reader["preferred_visit_day"]);
                            item.PaymentCycle = Convert.ToString(reader["payment_cycle"]);
                            item.CreditDays = Convert.ToInt32(reader["credit_days"]);
                            item.NextPaymentDate = reader["next_payment_date"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["next_payment_date"]);
                            item.Notes = Convert.ToString(reader["notes"]);
                            item.PurchaseAmount = Convert.ToDecimal(reader["total_purchase"]);
                            item.PurchaseDueAmount = Convert.ToDecimal(reader["total_due"]);
                            item.PaymentPaidAmount = Convert.ToDecimal(reader["total_paid"]);
                            item.ExpiryPendingCount = Convert.ToInt32(reader["expiry_pending_count"]);
                            item.ExpiryReturnedCount = Convert.ToInt32(reader["expiry_returned_count"]);
                            item.ExpiryBurntCount = Convert.ToInt32(reader["expiry_burnt_count"]);
                            item.NetBalance = CalculateNetBalance(item);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public int SaveVendor(VendorSaveRequest request)
        {
            ValidateVendor(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);
                int supplierId;

                using (MySqlCommand command = connection.CreateCommand())
                {
                    if (request.SupplierId.HasValue)
                    {
                        command.CommandText = @"
                            UPDATE suppliers
                            SET supplier_name = @supplierName,
                                phone = @phone,
                                address = @address,
                                opening_balance = @openingBalance,
                                balance_type = @balanceType,
                                is_active = @isActive,
                                preferred_visit_day = @preferredVisitDay,
                                payment_cycle = @paymentCycle,
                                credit_days = @creditDays,
                                next_payment_date = @nextPaymentDate,
                                notes = @notes
                            WHERE supplier_id = @supplierId;";
                        command.Parameters.AddWithValue("@supplierId", request.SupplierId.Value);
                        supplierId = request.SupplierId.Value;
                    }
                    else
                    {
                        command.CommandText = @"
                            INSERT INTO suppliers (
                                supplier_name, phone, address, opening_balance, balance_type, is_active,
                                preferred_visit_day, payment_cycle, credit_days, next_payment_date, notes
                            )
                            VALUES (
                                @supplierName, @phone, @address, @openingBalance, @balanceType, @isActive,
                                @preferredVisitDay, @paymentCycle, @creditDays, @nextPaymentDate, @notes
                            );";
                        supplierId = 0;
                    }

                    command.Parameters.AddWithValue("@supplierName", request.SupplierName.Trim());
                    command.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(request.Phone) ? (object)DBNull.Value : request.Phone.Trim());
                    command.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(request.Address) ? (object)DBNull.Value : request.Address.Trim());
                    command.Parameters.AddWithValue("@openingBalance", request.OpeningBalance);
                    command.Parameters.AddWithValue("@balanceType", string.IsNullOrWhiteSpace(request.BalanceType) ? "Payable" : request.BalanceType);
                    command.Parameters.AddWithValue("@isActive", request.IsActive);
                    command.Parameters.AddWithValue("@preferredVisitDay", string.IsNullOrWhiteSpace(request.PreferredVisitDay) ? (object)DBNull.Value : request.PreferredVisitDay);
                    command.Parameters.AddWithValue("@paymentCycle", string.IsNullOrWhiteSpace(request.PaymentCycle) ? (object)DBNull.Value : request.PaymentCycle);
                    command.Parameters.AddWithValue("@creditDays", request.CreditDays);
                    command.Parameters.AddWithValue("@nextPaymentDate", request.NextPaymentDate.HasValue ? (object)request.NextPaymentDate.Value.Date : DBNull.Value);
                    command.Parameters.AddWithValue("@notes", string.IsNullOrWhiteSpace(request.Notes) ? (object)DBNull.Value : request.Notes.Trim());
                    command.ExecuteNonQuery();

                    if (!request.SupplierId.HasValue)
                    {
                        supplierId = Convert.ToInt32(command.LastInsertedId);
                    }
                }

                return supplierId;
            }
        }

        public List<VendorProductLinkItem> GetVendorProducts(int? supplierId)
        {
            List<VendorProductLinkItem> items = new List<VendorProductLinkItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            p.product_id,
                            p.product_code,
                            p.product_name,
                            p.sale_price,
                            IFNULL(sp.last_purchase_price, 0.00) AS last_purchase_price,
                            CASE WHEN sp.supplier_id IS NULL THEN 0 ELSE 1 END AS is_linked,
                            IFNULL(sp.is_preferred, 0) AS is_preferred
                        FROM products p
                        LEFT JOIN supplier_products sp
                            ON sp.product_id = p.product_id
                           AND sp.supplier_id = @supplierId
                        WHERE p.is_active = 1
                        ORDER BY p.product_name ASC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId.HasValue ? (object)supplierId.Value : DBNull.Value);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorProductLinkItem item = new VendorProductLinkItem();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                            item.LastPurchasePrice = Convert.ToDecimal(reader["last_purchase_price"]);
                            item.IsLinked = Convert.ToBoolean(reader["is_linked"]);
                            item.IsPreferred = Convert.ToBoolean(reader["is_preferred"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<VendorProductLinkItem> GetLinkedVendorProducts(int supplierId)
        {
            List<VendorProductLinkItem> items = new List<VendorProductLinkItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            p.product_id,
                            p.product_code,
                            p.product_name,
                            p.sale_price,
                            IFNULL(sp.last_purchase_price, 0.00) AS last_purchase_price,
                            1 AS is_linked,
                            IFNULL(sp.is_preferred, 0) AS is_preferred
                        FROM supplier_products sp
                        INNER JOIN products p ON p.product_id = sp.product_id
                        WHERE sp.supplier_id = @supplierId
                          AND p.is_active = 1
                        ORDER BY p.product_name ASC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorProductLinkItem item = new VendorProductLinkItem();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                            item.LastPurchasePrice = Convert.ToDecimal(reader["last_purchase_price"]);
                            item.IsLinked = Convert.ToBoolean(reader["is_linked"]);
                            item.IsPreferred = Convert.ToBoolean(reader["is_preferred"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<VendorPurchaseHistoryItem> GetVendorPurchaseHistory(int supplierId)
        {
            List<VendorPurchaseHistoryItem> items = new List<VendorPurchaseHistoryItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            purchase_no,
                            purchase_date,
                            invoice_no,
                            grand_total,
                            paid_amount,
                            remaining_amount,
                            remarks
                        FROM purchase_header
                        WHERE supplier_id = @supplierId
                        ORDER BY purchase_date DESC, purchase_id DESC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorPurchaseHistoryItem item = new VendorPurchaseHistoryItem();
                            item.PurchaseNo = Convert.ToString(reader["purchase_no"]);
                            item.PurchaseDate = Convert.ToDateTime(reader["purchase_date"]);
                            item.InvoiceNo = Convert.ToString(reader["invoice_no"]);
                            item.GrandTotal = Convert.ToDecimal(reader["grand_total"]);
                            item.PaidAmount = Convert.ToDecimal(reader["paid_amount"]);
                            item.DueAmount = Convert.ToDecimal(reader["remaining_amount"]);
                            item.Remarks = Convert.ToString(reader["remarks"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<VendorLedgerItem> GetVendorLedger(int supplierId)
        {
            List<VendorLedgerItem> items = new List<VendorLedgerItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);

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
                                s.created_at AS entry_date,
                                'Opening' AS entry_type,
                                CONCAT('SUP-', s.supplier_id) AS reference_no,
                                CASE WHEN s.balance_type = 'Receivable' THEN s.opening_balance ELSE 0 END AS debit,
                                CASE WHEN s.balance_type = 'Payable' THEN s.opening_balance ELSE 0 END AS credit,
                                'Opening balance' AS remarks
                            FROM suppliers s
                            WHERE s.supplier_id = @supplierId

                            UNION ALL

                            SELECT
                                ph.purchase_date AS entry_date,
                                'Purchase' AS entry_type,
                                ph.purchase_no AS reference_no,
                                ph.paid_amount AS debit,
                                ph.grand_total AS credit,
                                IFNULL(ph.remarks, 'Vendor purchase') AS remarks
                            FROM purchase_header ph
                            WHERE ph.supplier_id = @supplierId

                            UNION ALL

                            SELECT
                                sp.payment_date AS entry_date,
                                'Payment' AS entry_type,
                                CONCAT('VP-', sp.supplier_payment_id) AS reference_no,
                                sp.amount AS debit,
                                0 AS credit,
                                IFNULL(sp.notes, 'Vendor payment') AS remarks
                            FROM supplier_payments sp
                            WHERE sp.supplier_id = @supplierId
                        ) ledger
                        ORDER BY entry_date DESC, reference_no DESC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorLedgerItem item = new VendorLedgerItem();
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

        public List<VendorExpiredReturnItem> GetVendorExpiredReturns(int supplierId)
        {
            List<VendorExpiredReturnItem> items = new List<VendorExpiredReturnItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureVendorEnhancements(connection);
                ExpiryService.EnsureExpirySchema(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            p.product_code,
                            p.product_name,
                            IFNULL(r.batch_no, '') AS batch_no,
                            r.expiry_date,
                            r.quantity,
                            r.resolution_status,
                            r.processed_at,
                            IFNULL(r.remarks, '') AS remarks
                        FROM expired_product_records r
                        INNER JOIN products p ON p.product_id = r.product_id
                        WHERE r.supplier_id = @supplierId
                        ORDER BY r.processed_at DESC, p.product_name ASC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VendorExpiredReturnItem item = new VendorExpiredReturnItem();
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.BatchNo = Convert.ToString(reader["batch_no"]);
                            item.ExpiryDate = reader["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["expiry_date"]);
                            item.Quantity = Convert.ToDecimal(reader["quantity"]);
                            item.ResolutionStatus = Convert.ToString(reader["resolution_status"]);
                            item.ProcessedAt = Convert.ToDateTime(reader["processed_at"]);
                            item.Remarks = Convert.ToString(reader["remarks"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public void SaveVendorProductLinks(int supplierId, List<VendorProductLinkItem> items)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureVendorEnhancements(connection);

                    using (MySqlCommand deleteCommand = connection.CreateCommand())
                    {
                        deleteCommand.Transaction = transaction;
                        deleteCommand.CommandText = "DELETE FROM supplier_products WHERE supplier_id = @supplierId;";
                        deleteCommand.Parameters.AddWithValue("@supplierId", supplierId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    int index;
                    for (index = 0; index < items.Count; index++)
                    {
                        VendorProductLinkItem item = items[index];
                        if (!item.IsLinked)
                        {
                            continue;
                        }

                        using (MySqlCommand insertCommand = connection.CreateCommand())
                        {
                            insertCommand.Transaction = transaction;
                            insertCommand.CommandText = @"
                                INSERT INTO supplier_products (
                                    supplier_id, product_id, last_purchase_price, is_preferred
                                )
                                VALUES (
                                    @supplierId, @productId, @lastPurchasePrice, @isPreferred
                                );";
                            insertCommand.Parameters.AddWithValue("@supplierId", supplierId);
                            insertCommand.Parameters.AddWithValue("@productId", item.ProductId);
                            insertCommand.Parameters.AddWithValue("@lastPurchasePrice", item.LastPurchasePrice);
                            insertCommand.Parameters.AddWithValue("@isPreferred", item.IsPreferred);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static void ValidateVendor(VendorSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrWhiteSpace(request.SupplierName))
            {
                throw new InvalidOperationException("Vendor name is required.");
            }
        }

        private static decimal CalculateNetBalance(VendorRecord item)
        {
            decimal openingEffect = item.BalanceType == "Payable"
                ? item.OpeningBalance
                : -item.OpeningBalance;

            return openingEffect + item.PurchaseDueAmount - item.PaymentPaidAmount;
        }

        private static void EnsureVendorEnhancements(MySqlConnection connection)
        {
            EnsureColumn(connection, "suppliers", "preferred_visit_day", "ALTER TABLE suppliers ADD COLUMN preferred_visit_day VARCHAR(20) NULL AFTER is_active;");
            EnsureColumn(connection, "suppliers", "payment_cycle", "ALTER TABLE suppliers ADD COLUMN payment_cycle VARCHAR(30) NULL AFTER preferred_visit_day;");
            EnsureColumn(connection, "suppliers", "credit_days", "ALTER TABLE suppliers ADD COLUMN credit_days INT NOT NULL DEFAULT 0 AFTER payment_cycle;");
            EnsureColumn(connection, "suppliers", "next_payment_date", "ALTER TABLE suppliers ADD COLUMN next_payment_date DATE NULL AFTER credit_days;");
            EnsureColumn(connection, "suppliers", "notes", "ALTER TABLE suppliers ADD COLUMN notes VARCHAR(255) NULL AFTER next_payment_date;");
            EnsureSupplierPaymentsTable(connection);
            EnsureSupplierProductsTable(connection);
        }

        private static void EnsureColumn(MySqlConnection connection, string tableName, string columnName, string ddl)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = @tableName
                      AND COLUMN_NAME = @columnName;";
                command.Parameters.AddWithValue("@tableName", tableName);
                command.Parameters.AddWithValue("@columnName", columnName);

                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = ddl;
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureSupplierProductsTable(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS supplier_products (
                        supplier_product_id INT AUTO_INCREMENT PRIMARY KEY,
                        supplier_id INT NOT NULL,
                        product_id INT NOT NULL,
                        last_purchase_price DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        is_preferred TINYINT(1) NOT NULL DEFAULT 1,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        UNIQUE KEY uq_supplier_product (supplier_id, product_id),
                        CONSTRAINT fk_supplier_products_supplier
                            FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_supplier_products_product
                            FOREIGN KEY (product_id) REFERENCES products(product_id)
                            ON DELETE CASCADE ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureSupplierPaymentsTable(MySqlConnection connection)
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
