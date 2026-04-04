using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class PurchaseService
    {
        public List<LookupOption> GetVendors()
        {
            return LoadLookupOptions(
                @"SELECT supplier_id, supplier_name
                  FROM suppliers
                  WHERE is_active = 1
                  ORDER BY supplier_name ASC;");
        }

        public List<LookupOption> GetWalletAccounts()
        {
            return LoadLookupOptions(
                @"SELECT wallet_account_id, account_name
                  FROM wallet_accounts
                  WHERE is_active = 1
                  ORDER BY account_name ASC;");
        }

        public List<PurchaseProductLookupItem> GetProducts(int? supplierId)
        {
            List<PurchaseProductLookupItem> items = new List<PurchaseProductLookupItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureSupplierProductsTable(connection);
                ExpiryService.EnsureExpirySchema(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            p.product_id,
                            p.product_code,
                            p.barcode,
                            p.product_name,
                            p.base_unit_id,
                            u.short_name,
                            p.purchase_price,
                            p.sale_price,
                            p.track_expiry,
                            p.default_shelf_life_days,
                            p.default_expiry_date,
                            sp.supplier_id AS preferred_vendor_id,
                            IFNULL(s.supplier_name, '') AS preferred_vendor_name
                        FROM products p
                        INNER JOIN units u ON u.unit_id = p.base_unit_id
                        LEFT JOIN supplier_products sp
                            ON sp.product_id = p.product_id
                           AND sp.is_preferred = 1
                        LEFT JOIN suppliers s ON s.supplier_id = sp.supplier_id
                        WHERE p.is_active = 1
                          AND (@supplierId IS NULL
                               OR sp.supplier_id = @supplierId
                               OR EXISTS
                               (
                                   SELECT 1
                                   FROM supplier_products spx
                                   WHERE spx.product_id = p.product_id
                                     AND spx.supplier_id = @supplierId
                               ))
                        ORDER BY
                            CASE WHEN sp.supplier_id = @supplierId THEN 0 ELSE 1 END,
                            p.product_name ASC;";
                    command.Parameters.AddWithValue("@supplierId", supplierId.HasValue ? (object)supplierId.Value : DBNull.Value);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PurchaseProductLookupItem item = new PurchaseProductLookupItem();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.Barcode = Convert.ToString(reader["barcode"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.UnitId = Convert.ToInt32(reader["base_unit_id"]);
                            item.UnitName = Convert.ToString(reader["short_name"]);
                            item.PurchasePrice = Convert.ToDecimal(reader["purchase_price"]);
                            item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                            item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                            item.DefaultShelfLifeDays = reader["default_shelf_life_days"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["default_shelf_life_days"]);
                            item.DefaultExpiryDate = reader["default_expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["default_expiry_date"]);
                            item.PreferredVendorId = reader["preferred_vendor_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["preferred_vendor_id"]);
                            item.PreferredVendorName = Convert.ToString(reader["preferred_vendor_name"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public PurchaseSaveResult SavePurchase(PurchaseSaveRequest request)
        {
            ValidateRequest(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureSupplierProductsTable(connection);
                    ExpiryService.EnsureExpirySchema(connection, transaction);

                    string purchaseNo = GenerateNextPurchaseNo(connection, transaction);
                    decimal subtotal = CalculateSubtotal(request.Items);
                    decimal grandTotal = subtotal - request.Discount + request.OtherCharges;
                    if (grandTotal < 0)
                    {
                        grandTotal = 0;
                    }

                    decimal remainingAmount = grandTotal - request.PaidAmount;
                    if (remainingAmount < 0)
                    {
                        remainingAmount = 0;
                    }

                    long purchaseId = InsertPurchaseHeader(connection, transaction, request, purchaseNo, subtotal, grandTotal, remainingAmount);
                    InsertPurchaseDetails(connection, transaction, purchaseId, request.Items);
                    InsertOrUpdateBatchRecords(connection, transaction, purchaseId, request.SupplierId, request.PurchaseDate, request.Items);
                    InsertStockLedgerEntries(connection, transaction, purchaseId, request.Items, request.UserId);
                    UpdateProductCosts(connection, transaction, request.Items);
                    UpdateSupplierProductLinks(connection, transaction, request.SupplierId, request.Items);

                    if (request.WalletAccountId.HasValue && request.PaidAmount > 0)
                    {
                        UpdateWalletBalance(connection, transaction, request.WalletAccountId.Value, request.PaidAmount);
                    }

                    AccountingService.PostPurchaseEntry(
                        connection,
                        transaction,
                        purchaseId,
                        grandTotal,
                        request.PaidAmount,
                        request.WalletAccountId,
                        request.Remarks,
                        request.UserId);

                    transaction.Commit();

                    PurchaseSaveResult result = new PurchaseSaveResult();
                    result.PurchaseId = purchaseId;
                    result.PurchaseNo = purchaseNo;
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private static void ValidateRequest(PurchaseSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.SupplierId <= 0)
            {
                throw new InvalidOperationException("Select a vendor.");
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new InvalidOperationException("Add at least one product.");
            }

            decimal grandTotal = CalculateSubtotal(request.Items) - request.Discount + request.OtherCharges;
            if (grandTotal < 0)
            {
                grandTotal = 0;
            }

            if (request.PaidAmount > grandTotal)
            {
                throw new InvalidOperationException("Paid amount cannot be greater than grand total.");
            }

            if (request.PaidAmount > 0 && !request.WalletAccountId.HasValue)
            {
                throw new InvalidOperationException("Select a wallet for paid amount.");
            }

            for (int i = 0; i < request.Items.Count; i++)
            {
                PurchaseCartItem item = request.Items[i];
                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException(string.Format("Quantity must be greater than zero for {0}.", item.ProductName));
                }

                if (item.Rate <= 0)
                {
                    throw new InvalidOperationException(string.Format("Rate must be greater than zero for {0}.", item.ProductName));
                }

                if (item.TrackExpiry && !item.ExpiryDate.HasValue)
                {
                    throw new InvalidOperationException(string.Format("Expiry date is required for {0}.", item.ProductName));
                }
            }
        }

        private static decimal CalculateSubtotal(List<PurchaseCartItem> items)
        {
            decimal subtotal = 0;
            for (int i = 0; i < items.Count; i++)
            {
                subtotal += items[i].LineTotal;
            }

            return subtotal;
        }

        private static string GenerateNextPurchaseNo(MySqlConnection connection, MySqlTransaction transaction)
        {
            int nextNumber = 1;

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT MAX(CAST(SUBSTRING(purchase_no, 5) AS UNSIGNED))
                    FROM purchase_header
                    WHERE purchase_no LIKE 'PUR-%';";
                object value = command.ExecuteScalar();
                if (!(value is DBNull) && value != null)
                {
                    nextNumber = Convert.ToInt32(value) + 1;
                }
            }

            return string.Format("PUR-{0:00000}", nextNumber);
        }

        private static long InsertPurchaseHeader(MySqlConnection connection, MySqlTransaction transaction, PurchaseSaveRequest request, string purchaseNo, decimal subtotal, decimal grandTotal, decimal remainingAmount)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    INSERT INTO purchase_header (
                        purchase_no, supplier_id, invoice_no, purchase_date, subtotal, discount, other_charges,
                        grand_total, paid_amount, remaining_amount, payment_wallet_account_id, remarks, created_by
                    )
                    VALUES (
                        @purchaseNo, @supplierId, @invoiceNo, @purchaseDate, @subtotal, @discount, @otherCharges,
                        @grandTotal, @paidAmount, @remainingAmount, @walletId, @remarks, @createdBy
                    );
                    SELECT LAST_INSERT_ID();";
                command.Parameters.AddWithValue("@purchaseNo", purchaseNo);
                command.Parameters.AddWithValue("@supplierId", request.SupplierId);
                command.Parameters.AddWithValue("@invoiceNo", string.IsNullOrWhiteSpace(request.InvoiceNo) ? (object)DBNull.Value : request.InvoiceNo.Trim());
                command.Parameters.AddWithValue("@purchaseDate", request.PurchaseDate);
                command.Parameters.AddWithValue("@subtotal", subtotal);
                command.Parameters.AddWithValue("@discount", request.Discount);
                command.Parameters.AddWithValue("@otherCharges", request.OtherCharges);
                command.Parameters.AddWithValue("@grandTotal", grandTotal);
                command.Parameters.AddWithValue("@paidAmount", request.PaidAmount);
                command.Parameters.AddWithValue("@remainingAmount", remainingAmount);
                command.Parameters.AddWithValue("@walletId", (object)request.WalletAccountId ?? DBNull.Value);
                command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                command.Parameters.AddWithValue("@createdBy", request.UserId);
                return Convert.ToInt64(command.ExecuteScalar());
            }
        }

        private static void InsertPurchaseDetails(MySqlConnection connection, MySqlTransaction transaction, long purchaseId, List<PurchaseCartItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PurchaseCartItem item = items[i];
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO purchase_detail (
                            purchase_id, product_id, unit_id, quantity, rate, line_total, batch_no, expiry_date
                        )
                        VALUES (
                            @purchaseId, @productId, @unitId, @quantity, @rate, @lineTotal, @batchNo, @expiryDate
                        );
                        SELECT LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@purchaseId", purchaseId);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@unitId", item.UnitId);
                    command.Parameters.AddWithValue("@quantity", item.Quantity);
                    command.Parameters.AddWithValue("@rate", item.Rate);
                    command.Parameters.AddWithValue("@lineTotal", item.LineTotal);
                    command.Parameters.AddWithValue("@batchNo", string.IsNullOrWhiteSpace(item.BatchNo) ? (object)DBNull.Value : item.BatchNo.Trim());
                    command.Parameters.AddWithValue("@expiryDate", (object)item.ExpiryDate ?? DBNull.Value);
                    item.PurchaseDetailId = Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }

        private static void InsertOrUpdateBatchRecords(MySqlConnection connection, MySqlTransaction transaction, long purchaseId, int supplierId, DateTime purchaseDate, List<PurchaseCartItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PurchaseCartItem item = items[i];
                if (!item.TrackExpiry)
                {
                    continue;
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO product_stock_batches (
                            product_id, supplier_id, purchase_id, purchase_detail_id, batch_no, expiry_date,
                            received_qty, remaining_qty, unit_cost, status, received_at, updated_at
                        )
                        VALUES (
                            @productId, @supplierId, @purchaseId, @purchaseDetailId, @batchNo, @expiryDate,
                            @receivedQty, @remainingQty, @unitCost, 'Active', @receivedAt, NOW()
                        );";
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@supplierId", supplierId);
                    command.Parameters.AddWithValue("@purchaseId", purchaseId);
                    command.Parameters.AddWithValue("@purchaseDetailId", item.PurchaseDetailId);
                    command.Parameters.AddWithValue("@batchNo", string.IsNullOrWhiteSpace(item.BatchNo) ? (object)DBNull.Value : item.BatchNo.Trim());
                    command.Parameters.AddWithValue("@expiryDate", (object)item.ExpiryDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@receivedQty", item.Quantity);
                    command.Parameters.AddWithValue("@remainingQty", item.Quantity);
                    command.Parameters.AddWithValue("@unitCost", item.Rate);
                    command.Parameters.AddWithValue("@receivedAt", purchaseDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void InsertStockLedgerEntries(MySqlConnection connection, MySqlTransaction transaction, long purchaseId, List<PurchaseCartItem> items, int userId)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PurchaseCartItem item = items[i];
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO stock_ledger (
                            product_id, transaction_type, reference_id, reference_table,
                            qty_in, qty_out, unit_cost, remarks, created_by
                        )
                        VALUES (
                            @productId, 'Purchase', @referenceId, 'purchase_header',
                            @qtyIn, 0.00, @unitCost, @remarks, @createdBy
                        );";
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@referenceId", purchaseId);
                    command.Parameters.AddWithValue("@qtyIn", item.Quantity);
                    command.Parameters.AddWithValue("@unitCost", item.Rate);
                    command.Parameters.AddWithValue("@remarks", "Purchase transaction");
                    command.Parameters.AddWithValue("@createdBy", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateProductCosts(MySqlConnection connection, MySqlTransaction transaction, List<PurchaseCartItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PurchaseCartItem item = items[i];
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        UPDATE products
                        SET purchase_price = @purchasePrice,
                            updated_at = NOW()
                        WHERE product_id = @productId;";
                    command.Parameters.AddWithValue("@purchasePrice", item.Rate);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateSupplierProductLinks(MySqlConnection connection, MySqlTransaction transaction, int supplierId, List<PurchaseCartItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                PurchaseCartItem item = items[i];
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO supplier_products (
                            supplier_id, product_id, last_purchase_price, is_preferred
                        )
                        VALUES (
                            @supplierId, @productId, @lastPurchasePrice, 1
                        )
                        ON DUPLICATE KEY UPDATE
                            last_purchase_price = VALUES(last_purchase_price);";
                    command.Parameters.AddWithValue("@supplierId", supplierId);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@lastPurchasePrice", item.Rate);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void UpdateWalletBalance(MySqlConnection connection, MySqlTransaction transaction, int walletAccountId, decimal amount)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE wallet_accounts
                    SET current_balance = current_balance - @amount
                    WHERE wallet_account_id = @walletId;";
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@walletId", walletAccountId);
                command.ExecuteNonQuery();
            }
        }

        private static List<LookupOption> LoadLookupOptions(string sql)
        {
            List<LookupOption> items = new List<LookupOption>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LookupOption item = new LookupOption();
                        item.Id = Convert.ToInt32(reader.GetValue(0));
                        item.Name = Convert.ToString(reader.GetValue(1));
                        items.Add(item);
                    }
                }
            }

            return items;
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
    }
}
