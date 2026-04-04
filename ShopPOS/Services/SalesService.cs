using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class SalesService
    {
        public SaleEditRecord GetSaleForEdit(long saleId)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureSaleTrackingColumns(connection, null);
                ExpiryService.EnsureExpirySchema(connection, null);

                SaleEditRecord record = null;
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            sale_id,
                            sale_no,
                            sale_date,
                            customer_id,
                            payment_method,
                            payment_wallet_account_id,
                            remarks,
                            discount,
                            extra_charges,
                            paid_amount,
                            IFNULL(is_refunded, 0) AS is_refunded
                        FROM sale_header
                        WHERE sale_id = @saleId;";
                    command.Parameters.AddWithValue("@saleId", saleId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException("Selected grocery sale was not found.");
                        }

                        record = new SaleEditRecord();
                        record.SaleId = Convert.ToInt64(reader["sale_id"]);
                        record.SaleNo = Convert.ToString(reader["sale_no"]);
                        record.SaleDate = Convert.ToDateTime(reader["sale_date"]);
                        record.CustomerId = reader["customer_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["customer_id"]);
                        record.PaymentMethod = Convert.ToString(reader["payment_method"]);
                        record.WalletAccountId = reader["payment_wallet_account_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["payment_wallet_account_id"]);
                        record.Remarks = Convert.ToString(reader["remarks"]);
                        record.Discount = Convert.ToDecimal(reader["discount"]);
                        record.ExtraCharges = Convert.ToDecimal(reader["extra_charges"]);
                        record.PaidAmount = Convert.ToDecimal(reader["paid_amount"]);
                        record.IsRefunded = Convert.ToBoolean(reader["is_refunded"]);
                    }
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            sd.product_id,
                            p.product_code,
                            p.product_name,
                            sd.unit_id,
                            u.short_name,
                            sd.quantity,
                            sd.rate,
                            sd.cost_rate,
                            p.track_stock,
                            p.track_expiry,
                            (
                                SELECT IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00)
                                FROM stock_ledger sl
                                WHERE sl.product_id = sd.product_id
                            ) AS current_stock
                        FROM sale_detail sd
                        INNER JOIN products p ON p.product_id = sd.product_id
                        INNER JOIN units u ON u.unit_id = sd.unit_id
                        WHERE sd.sale_id = @saleId;";
                    command.Parameters.AddWithValue("@saleId", saleId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SaleCartItem item = new SaleCartItem();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.UnitId = Convert.ToInt32(reader["unit_id"]);
                            item.UnitName = Convert.ToString(reader["short_name"]);
                            item.Quantity = Convert.ToDecimal(reader["quantity"]);
                            item.Rate = Convert.ToDecimal(reader["rate"]);
                            item.CostRate = Convert.ToDecimal(reader["cost_rate"]);
                            item.TrackStock = Convert.ToBoolean(reader["track_stock"]);
                            item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                            item.AvailableStock = Convert.ToDecimal(reader["current_stock"]) + item.Quantity;
                            record.Items.Add(item);
                        }
                    }
                }

                return record;
            }
        }

        public List<SaleProductLookupItem> GetProducts()
        {
            List<SaleProductLookupItem> products = new List<SaleProductLookupItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                ExpiryService.EnsureExpirySchema(connection, null);
                command.CommandText = @"
                    SELECT
                        p.product_id,
                        p.product_code,
                        p.barcode,
                        p.product_name,
                        p.base_unit_id,
                        u.short_name,
                        p.sale_price,
                        p.purchase_price,
                        p.track_stock,
                        p.track_expiry,
                        CASE
                            WHEN IFNULL(p.track_expiry, 0) = 1 THEN
                                IFNULL((
                                    SELECT SUM(b.remaining_qty)
                                    FROM product_stock_batches b
                                    WHERE b.product_id = p.product_id
                                      AND b.remaining_qty > 0
                                      AND b.status = 'Active'
                                      AND (b.expiry_date IS NULL OR b.expiry_date >= CURDATE())
                                ), 0.00)
                            ELSE IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00)
                        END AS current_stock
                    FROM products p
                    INNER JOIN units u ON u.unit_id = p.base_unit_id
                    LEFT JOIN stock_ledger sl ON sl.product_id = p.product_id
                    WHERE p.is_active = 1
                    GROUP BY
                        p.product_id, p.product_code, p.barcode, p.product_name,
                        p.base_unit_id, u.short_name, p.sale_price, p.purchase_price, p.track_stock, p.track_expiry
                    ORDER BY p.product_name ASC;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SaleProductLookupItem item = new SaleProductLookupItem();
                        item.ProductId = Convert.ToInt32(reader["product_id"]);
                        item.ProductCode = Convert.ToString(reader["product_code"]);
                        item.Barcode = Convert.ToString(reader["barcode"]);
                        item.ProductName = Convert.ToString(reader["product_name"]);
                        item.UnitId = Convert.ToInt32(reader["base_unit_id"]);
                        item.UnitName = Convert.ToString(reader["short_name"]);
                        item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                        item.PurchasePrice = Convert.ToDecimal(reader["purchase_price"]);
                        item.TrackStock = Convert.ToBoolean(reader["track_stock"]);
                        item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                        item.CurrentStock = Convert.ToDecimal(reader["current_stock"]);
                        products.Add(item);
                    }
                }
            }

            return products;
        }

        public List<LookupOption> GetCustomers()
        {
            return LoadLookupOptions(
                @"SELECT customer_id, customer_name
                  FROM customers
                  WHERE is_active = 1
                  ORDER BY customer_name ASC;");
        }

        public List<LookupOption> GetWalletAccounts()
        {
            return LoadLookupOptions(
                @"SELECT wallet_account_id, account_name
                  FROM wallet_accounts
                  WHERE is_active = 1
                  ORDER BY account_name ASC;");
        }

        public SaleSaveResult SaveSale(SaleSaveRequest request)
        {
            ValidateRequest(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureSaleTrackingColumns(connection, transaction);
                    ExpiryService.EnsureExpirySchema(connection, transaction);
                    EnsureStockAvailability(connection, transaction, request.Items);

                    string saleNo = GenerateNextSaleNo(connection, transaction);
                    decimal subtotal = CalculateSubtotal(request.Items);
                    decimal grandTotal = subtotal - request.Discount + request.ExtraCharges;
                    decimal changeAmount = request.PaidAmount > grandTotal
                        ? request.PaidAmount - grandTotal
                        : 0;

                    long saleId = InsertSaleHeader(connection, transaction, request, saleNo, subtotal, grandTotal, changeAmount);
                    InsertSaleDetails(connection, transaction, request.Items, saleId);
                    InsertStockLedgerEntries(connection, transaction, request.Items, saleId, request.UserId);

                    if (request.WalletAccountId.HasValue && request.PaidAmount > 0)
                    {
                        UpdateWalletBalance(connection, transaction, request.WalletAccountId.Value, request.PaidAmount);
                    }

                    decimal totalCost = CalculateTotalCost(request.Items);

                    AccountingService.PostSaleEntry(
                        connection,
                        transaction,
                        saleId,
                        grandTotal,
                        totalCost,
                        request.PaidAmount,
                        request.WalletAccountId,
                        request.Remarks,
                        request.UserId);

                    transaction.Commit();

                    SaleSaveResult result = new SaleSaveResult();
                    result.SaleId = saleId;
                    result.SaleNo = saleNo;
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateSale(long saleId, SaleSaveRequest request)
        {
            ValidateRequest(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureSaleTrackingColumns(connection, transaction);
                    ExpiryService.EnsureExpirySchema(connection, transaction);
                    SaleEditRecord existing = GetSaleForEdit(connection, transaction, saleId);
                    if (existing.IsRefunded)
                    {
                        throw new InvalidOperationException("Refunded sales cannot be edited.");
                    }

                    RestoreSaleBatchAllocations(connection, transaction, saleId);
                    DeleteSaleDetails(connection, transaction, saleId);
                    DeleteSaleStockLedger(connection, transaction, saleId);
                    ReverseWalletReceipt(connection, transaction, existing.WalletAccountId, existing.PaidAmount);
                    AccountingService.DeleteVouchers(connection, transaction, "sale_header", saleId);
                    EnsureStockAvailability(connection, transaction, request.Items);

                    decimal subtotal = CalculateSubtotal(request.Items);
                    decimal grandTotal = subtotal - request.Discount + request.ExtraCharges;
                    decimal changeAmount = request.PaidAmount > grandTotal
                        ? request.PaidAmount - grandTotal
                        : 0;

                    UpdateSaleHeader(connection, transaction, saleId, request, subtotal, grandTotal, changeAmount);
                    InsertSaleDetails(connection, transaction, request.Items, saleId);
                    InsertStockLedgerEntries(connection, transaction, request.Items, saleId, request.UserId);

                    if (request.WalletAccountId.HasValue && request.PaidAmount > 0)
                    {
                        UpdateWalletBalance(connection, transaction, request.WalletAccountId.Value, request.PaidAmount);
                    }

                    decimal totalCost = CalculateTotalCost(request.Items);
                    AccountingService.PostSaleEntry(
                        connection,
                        transaction,
                        saleId,
                        grandTotal,
                        totalCost,
                        request.PaidAmount,
                        request.WalletAccountId,
                        request.Remarks,
                        request.UserId);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void RefundSale(long saleId, int userId, string remarks)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureSaleTrackingColumns(connection, transaction);
                    ExpiryService.EnsureExpirySchema(connection, transaction);
                    SaleEditRecord existing = GetSaleForEdit(connection, transaction, saleId);
                    if (existing.IsRefunded)
                    {
                        throw new InvalidOperationException("This grocery sale is already refunded.");
                    }

                    RestoreSaleBatchAllocations(connection, transaction, saleId);
                    InsertSaleRefundStockLedger(connection, transaction, existing.Items, saleId, userId);
                    ReverseWalletReceipt(connection, transaction, existing.WalletAccountId, existing.PaidAmount);

                    decimal grandTotal = CalculateSubtotal(existing.Items) - existing.Discount + existing.ExtraCharges;
                    if (grandTotal < 0)
                    {
                        grandTotal = 0;
                    }

                    decimal totalCost = CalculateTotalCost(existing.Items);
                    AccountingService.PostSaleRefundEntry(
                        connection,
                        transaction,
                        saleId,
                        grandTotal,
                        totalCost,
                        existing.PaidAmount,
                        existing.WalletAccountId,
                        remarks,
                        userId);

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            UPDATE sale_header
                            SET
                                is_refunded = 1,
                                refunded_at = NOW(),
                                refunded_by = @userId,
                                refund_remarks = @remarks
                            WHERE sale_id = @saleId;";
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                        command.Parameters.AddWithValue("@saleId", saleId);
                        command.ExecuteNonQuery();
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

        private static void ValidateRequest(SaleSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Items == null || request.Items.Count == 0)
            {
                throw new InvalidOperationException("Add at least one product to the cart.");
            }

            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                throw new InvalidOperationException("Select a payment method.");
            }

            decimal subtotal = CalculateSubtotal(request.Items);
            decimal grandTotal = subtotal - request.Discount + request.ExtraCharges;
            if (grandTotal < 0)
            {
                grandTotal = 0;
            }

            if (request.PaidAmount > grandTotal)
            {
                throw new InvalidOperationException("Paid amount cannot be greater than grand total.");
            }

            bool isFullCredit = request.PaymentMethod == "Credit";
            bool isPartialCredit = request.PaymentMethod == "Partial Credit";
            bool hasCredit = request.PaidAmount < grandTotal || isFullCredit || isPartialCredit;
            if (hasCredit && !request.CustomerId.HasValue)
            {
                throw new InvalidOperationException("Select a customer for credit or partial-payment sales.");
            }

            if (isFullCredit)
            {
                request.WalletAccountId = null;
                request.PaidAmount = 0;
            }

            if (request.PaidAmount > 0 && !request.WalletAccountId.HasValue && !isFullCredit)
            {
                throw new InvalidOperationException("Select a wallet account for received payment.");
            }

            if (isPartialCredit && request.PaidAmount <= 0)
            {
                throw new InvalidOperationException("Enter the received amount for a partial-credit sale.");
            }

            if (isPartialCredit && request.PaidAmount >= grandTotal)
            {
                throw new InvalidOperationException("Partial-credit sale must have some due amount remaining.");
            }

            int index;
            for (index = 0; index < request.Items.Count; index++)
            {
                SaleCartItem item = request.Items[index];

                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException(string.Format("Quantity must be greater than zero for {0}.", item.ProductName));
                }

                if (item.Rate <= 0)
                {
                    throw new InvalidOperationException(string.Format("Rate must be greater than zero for {0}.", item.ProductName));
                }
            }
        }

        private static decimal CalculateSubtotal(List<SaleCartItem> items)
        {
            decimal subtotal = 0;
            int index;

            for (index = 0; index < items.Count; index++)
            {
                subtotal += items[index].LineTotal;
            }

            return subtotal;
        }

        private static decimal CalculateTotalCost(List<SaleCartItem> items)
        {
            decimal totalCost = 0;
            int index;

            for (index = 0; index < items.Count; index++)
            {
                totalCost += items[index].CostRate * items[index].Quantity;
            }

            return totalCost;
        }

        private static void EnsureStockAvailability(MySqlConnection connection, MySqlTransaction transaction, List<SaleCartItem> items)
        {
            int index;

            for (index = 0; index < items.Count; index++)
            {
                SaleCartItem item = items[index];
                if (!item.TrackStock)
                {
                    continue;
                }

                decimal availableStock;
                if (item.TrackExpiry)
                {
                    availableStock = GetAvailableBatchStock(connection, transaction, item.ProductId);
                }
                else
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            SELECT IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00)
                            FROM stock_ledger sl
                            WHERE sl.product_id = @productId;";
                        command.Parameters.AddWithValue("@productId", item.ProductId);
                        availableStock = Convert.ToDecimal(command.ExecuteScalar());
                    }
                }

                if (availableStock < item.Quantity)
                {
                    throw new InvalidOperationException(
                        string.Format("Insufficient stock for {0}. Available: {1:N2}", item.ProductName, availableStock));
                }
            }
        }

        private static string GenerateNextSaleNo(MySqlConnection connection, MySqlTransaction transaction)
        {
            int nextNumber = 1;

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT MAX(CAST(SUBSTRING(sale_no, 5) AS UNSIGNED))
                    FROM sale_header
                    WHERE sale_no LIKE 'SAL-%';";

                object value = command.ExecuteScalar();
                if (!(value is DBNull) && value != null)
                {
                    nextNumber = Convert.ToInt32(value) + 1;
                }
            }

            return string.Format("SAL-{0:00000}", nextNumber);
        }

        private static long InsertSaleHeader(
            MySqlConnection connection,
            MySqlTransaction transaction,
            SaleSaveRequest request,
            string saleNo,
            decimal subtotal,
            decimal grandTotal,
            decimal changeAmount)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    INSERT INTO sale_header (
                        sale_no, sale_date, customer_id, subtotal, discount, extra_charges,
                        grand_total, paid_amount, change_amount, payment_method,
                        payment_wallet_account_id, remarks, shift_id, created_by
                    )
                    VALUES (
                        @saleNo, @saleDate, @customerId, @subtotal, @discount, @extraCharges,
                        @grandTotal, @paidAmount, @changeAmount, @paymentMethod,
                        @walletId, @remarks, NULL, @createdBy
                    );
                    SELECT LAST_INSERT_ID();";

                command.Parameters.AddWithValue("@saleNo", saleNo);
                command.Parameters.AddWithValue("@saleDate", request.SaleDate);
                command.Parameters.AddWithValue("@customerId", (object)request.CustomerId ?? DBNull.Value);
                command.Parameters.AddWithValue("@subtotal", subtotal);
                command.Parameters.AddWithValue("@discount", request.Discount);
                command.Parameters.AddWithValue("@extraCharges", request.ExtraCharges);
                command.Parameters.AddWithValue("@grandTotal", grandTotal);
                command.Parameters.AddWithValue("@paidAmount", request.PaidAmount);
                command.Parameters.AddWithValue("@changeAmount", changeAmount);
                command.Parameters.AddWithValue("@paymentMethod", request.PaymentMethod);
                command.Parameters.AddWithValue("@walletId", (object)request.WalletAccountId ?? DBNull.Value);
                command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                command.Parameters.AddWithValue("@createdBy", request.UserId);

                return Convert.ToInt64(command.ExecuteScalar());
            }
        }

        private static void InsertSaleDetails(MySqlConnection connection, MySqlTransaction transaction, List<SaleCartItem> items, long saleId)
        {
            int index;

            for (index = 0; index < items.Count; index++)
            {
                SaleCartItem item = items[index];
                List<BatchAllocationEntry> allocations = null;

                if (item.TrackStock && item.TrackExpiry)
                {
                    allocations = AllocateSaleBatches(connection, transaction, saleId, item);
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO sale_detail (
                            sale_id, product_id, unit_id, quantity, rate, cost_rate, line_total, profit_amount
                        )
                        VALUES (
                            @saleId, @productId, @unitId, @quantity, @rate, @costRate, @lineTotal, @profitAmount
                        );";

                    decimal costRate = item.CostRate;
                    if (allocations != null && allocations.Count > 0)
                    {
                        decimal totalAllocatedCost = 0;
                        for (int allocationIndex = 0; allocationIndex < allocations.Count; allocationIndex++)
                        {
                            totalAllocatedCost += allocations[allocationIndex].UnitCost * allocations[allocationIndex].Quantity;
                        }

                        costRate = totalAllocatedCost / item.Quantity;
                        item.CostRate = costRate;
                    }

                    command.Parameters.AddWithValue("@saleId", saleId);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@unitId", item.UnitId);
                    command.Parameters.AddWithValue("@quantity", item.Quantity);
                    command.Parameters.AddWithValue("@rate", item.Rate);
                    command.Parameters.AddWithValue("@costRate", costRate);
                    command.Parameters.AddWithValue("@lineTotal", item.LineTotal);
                    command.Parameters.AddWithValue("@profitAmount", (item.Rate - costRate) * item.Quantity);
                    command.ExecuteNonQuery();

                    if (allocations != null && allocations.Count > 0)
                    {
                        long saleDetailId = command.LastInsertedId;
                        InsertSaleBatchAllocations(connection, transaction, saleId, saleDetailId, item.ProductId, allocations);
                    }
                }
            }
        }

        private static void InsertStockLedgerEntries(MySqlConnection connection, MySqlTransaction transaction, List<SaleCartItem> items, long saleId, int userId)
        {
            int index;

            for (index = 0; index < items.Count; index++)
            {
                SaleCartItem item = items[index];
                if (!item.TrackStock)
                {
                    continue;
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO stock_ledger (
                            product_id, transaction_type, reference_id, reference_table,
                            qty_in, qty_out, unit_cost, remarks, created_by
                        )
                        VALUES (
                            @productId, 'Sale', @referenceId, 'sale_header',
                            0.00, @qtyOut, @unitCost, @remarks, @createdBy
                        );";

                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@referenceId", saleId);
                    command.Parameters.AddWithValue("@qtyOut", item.Quantity);
                    command.Parameters.AddWithValue("@unitCost", item.CostRate);
                    command.Parameters.AddWithValue("@remarks", "Sale transaction");
                    command.Parameters.AddWithValue("@createdBy", userId);
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
                    SET current_balance = current_balance + @amount
                    WHERE wallet_account_id = @walletId;";

                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@walletId", walletAccountId);
                command.ExecuteNonQuery();
            }
        }

        internal static void EnsureSaleTrackingColumns(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    ALTER TABLE sale_header
                    MODIFY COLUMN payment_method ENUM('Cash','EasyPaisa','JazzCash','Bank','Mixed','Credit','Partial Credit')
                    NOT NULL DEFAULT 'Cash';";
                command.ExecuteNonQuery();
            }

            EnsureColumn(connection, transaction, "sale_header", "is_refunded", "ALTER TABLE sale_header ADD COLUMN is_refunded TINYINT(1) NOT NULL DEFAULT 0 AFTER payment_wallet_account_id;");
            EnsureColumn(connection, transaction, "sale_header", "refunded_at", "ALTER TABLE sale_header ADD COLUMN refunded_at DATETIME NULL AFTER is_refunded;");
            EnsureColumn(connection, transaction, "sale_header", "refunded_by", "ALTER TABLE sale_header ADD COLUMN refunded_by INT NULL AFTER refunded_at;");
            EnsureColumn(connection, transaction, "sale_header", "refund_remarks", "ALTER TABLE sale_header ADD COLUMN refund_remarks VARCHAR(255) NULL AFTER refunded_by;");
            EnsureColumn(connection, transaction, "sale_header", "edited_at", "ALTER TABLE sale_header ADD COLUMN edited_at DATETIME NULL AFTER refund_remarks;");
            EnsureColumn(connection, transaction, "sale_header", "edited_by", "ALTER TABLE sale_header ADD COLUMN edited_by INT NULL AFTER edited_at;");
        }

        private static void EnsureColumn(MySqlConnection connection, MySqlTransaction transaction, string tableName, string columnName, string alterSql)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = @tableName
                      AND COLUMN_NAME = @columnName;";
                command.Parameters.AddWithValue("@tableName", tableName);
                command.Parameters.AddWithValue("@columnName", columnName);
                bool exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
                if (exists)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = alterSql;
                command.ExecuteNonQuery();
            }
        }

        private static SaleEditRecord GetSaleForEdit(MySqlConnection connection, MySqlTransaction transaction, long saleId)
        {
            SaleEditRecord record = null;

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT
                        sale_id, sale_no, sale_date, customer_id, payment_method,
                        payment_wallet_account_id, remarks, discount, extra_charges, paid_amount,
                        IFNULL(is_refunded, 0) AS is_refunded
                    FROM sale_header
                    WHERE sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleId", saleId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected grocery sale was not found.");
                    }

                    record = new SaleEditRecord();
                    record.SaleId = Convert.ToInt64(reader["sale_id"]);
                    record.SaleNo = Convert.ToString(reader["sale_no"]);
                    record.SaleDate = Convert.ToDateTime(reader["sale_date"]);
                    record.CustomerId = reader["customer_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["customer_id"]);
                    record.PaymentMethod = Convert.ToString(reader["payment_method"]);
                    record.WalletAccountId = reader["payment_wallet_account_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["payment_wallet_account_id"]);
                    record.Remarks = Convert.ToString(reader["remarks"]);
                    record.Discount = Convert.ToDecimal(reader["discount"]);
                    record.ExtraCharges = Convert.ToDecimal(reader["extra_charges"]);
                    record.PaidAmount = Convert.ToDecimal(reader["paid_amount"]);
                    record.IsRefunded = Convert.ToBoolean(reader["is_refunded"]);
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT
                        sd.product_id,
                        p.product_code,
                        p.product_name,
                        sd.unit_id,
                        u.short_name,
                        sd.quantity,
                        sd.rate,
                        sd.cost_rate,
                        p.track_stock,
                        p.track_expiry
                    FROM sale_detail sd
                    INNER JOIN products p ON p.product_id = sd.product_id
                    INNER JOIN units u ON u.unit_id = sd.unit_id
                    WHERE sd.sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleId", saleId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SaleCartItem item = new SaleCartItem();
                        item.ProductId = Convert.ToInt32(reader["product_id"]);
                        item.ProductCode = Convert.ToString(reader["product_code"]);
                        item.ProductName = Convert.ToString(reader["product_name"]);
                        item.UnitId = Convert.ToInt32(reader["unit_id"]);
                        item.UnitName = Convert.ToString(reader["short_name"]);
                        item.Quantity = Convert.ToDecimal(reader["quantity"]);
                        item.Rate = Convert.ToDecimal(reader["rate"]);
                        item.CostRate = Convert.ToDecimal(reader["cost_rate"]);
                        item.TrackStock = Convert.ToBoolean(reader["track_stock"]);
                        item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                        record.Items.Add(item);
                    }
                }
            }

            return record;
        }

        private static void DeleteSaleDetails(MySqlConnection connection, MySqlTransaction transaction, long saleId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM sale_detail WHERE sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleId", saleId);
                command.ExecuteNonQuery();
            }
        }

        private static void DeleteSaleStockLedger(MySqlConnection connection, MySqlTransaction transaction, long saleId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    DELETE FROM stock_ledger
                    WHERE reference_table = 'sale_header'
                      AND reference_id = @saleId
                      AND transaction_type = 'Sale';";
                command.Parameters.AddWithValue("@saleId", saleId);
                command.ExecuteNonQuery();
            }
        }

        private static void ReverseWalletReceipt(MySqlConnection connection, MySqlTransaction transaction, int? walletAccountId, decimal amount)
        {
            if (!walletAccountId.HasValue || amount <= 0)
            {
                return;
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE wallet_accounts
                    SET current_balance = current_balance - @amount
                    WHERE wallet_account_id = @walletId;";
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@walletId", walletAccountId.Value);
                command.ExecuteNonQuery();
            }
        }

        private static void UpdateSaleHeader(MySqlConnection connection, MySqlTransaction transaction, long saleId, SaleSaveRequest request, decimal subtotal, decimal grandTotal, decimal changeAmount)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE sale_header
                    SET
                        sale_date = @saleDate,
                        customer_id = @customerId,
                        subtotal = @subtotal,
                        discount = @discount,
                        extra_charges = @extraCharges,
                        grand_total = @grandTotal,
                        paid_amount = @paidAmount,
                        change_amount = @changeAmount,
                        payment_method = @paymentMethod,
                        payment_wallet_account_id = @walletId,
                        remarks = @remarks,
                        edited_at = NOW(),
                        edited_by = @editedBy
                    WHERE sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleDate", request.SaleDate);
                command.Parameters.AddWithValue("@customerId", (object)request.CustomerId ?? DBNull.Value);
                command.Parameters.AddWithValue("@subtotal", subtotal);
                command.Parameters.AddWithValue("@discount", request.Discount);
                command.Parameters.AddWithValue("@extraCharges", request.ExtraCharges);
                command.Parameters.AddWithValue("@grandTotal", grandTotal);
                command.Parameters.AddWithValue("@paidAmount", request.PaidAmount);
                command.Parameters.AddWithValue("@changeAmount", changeAmount);
                command.Parameters.AddWithValue("@paymentMethod", request.PaymentMethod);
                command.Parameters.AddWithValue("@walletId", (object)request.WalletAccountId ?? DBNull.Value);
                command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                command.Parameters.AddWithValue("@editedBy", request.UserId);
                command.Parameters.AddWithValue("@saleId", saleId);
                command.ExecuteNonQuery();
            }
        }

        private static void InsertSaleRefundStockLedger(MySqlConnection connection, MySqlTransaction transaction, List<SaleCartItem> items, long saleId, int userId)
        {
            for (int index = 0; index < items.Count; index++)
            {
                SaleCartItem item = items[index];
                if (!item.TrackStock)
                {
                    continue;
                }

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO stock_ledger (
                            product_id, transaction_type, reference_id, reference_table,
                            qty_in, qty_out, unit_cost, remarks, created_by
                        )
                        VALUES (
                            @productId, 'SaleRefund', @referenceId, 'sale_header',
                            @qtyIn, 0.00, @unitCost, @remarks, @createdBy
                        );";
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@referenceId", saleId);
                    command.Parameters.AddWithValue("@qtyIn", item.Quantity);
                    command.Parameters.AddWithValue("@unitCost", item.CostRate);
                    command.Parameters.AddWithValue("@remarks", "Sale refund");
                    command.Parameters.AddWithValue("@createdBy", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static decimal GetAvailableBatchStock(MySqlConnection connection, MySqlTransaction transaction, int productId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT IFNULL(SUM(remaining_qty), 0.00)
                    FROM product_stock_batches
                    WHERE product_id = @productId
                      AND remaining_qty > 0
                      AND status = 'Active'
                      AND (expiry_date IS NULL OR expiry_date >= CURDATE());";
                command.Parameters.AddWithValue("@productId", productId);
                return Convert.ToDecimal(command.ExecuteScalar());
            }
        }

        private static List<BatchAllocationEntry> AllocateSaleBatches(MySqlConnection connection, MySqlTransaction transaction, long saleId, SaleCartItem item)
        {
            List<BatchAllocationEntry> allocations = new List<BatchAllocationEntry>();
            decimal remaining = item.Quantity;

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT batch_id, remaining_qty, unit_cost
                    FROM product_stock_batches
                    WHERE product_id = @productId
                      AND remaining_qty > 0
                      AND status = 'Active'
                      AND (expiry_date IS NULL OR expiry_date >= CURDATE())
                    ORDER BY
                        CASE WHEN expiry_date IS NULL THEN 1 ELSE 0 END,
                        expiry_date ASC,
                        received_at ASC,
                        batch_id ASC;";
                command.Parameters.AddWithValue("@productId", item.ProductId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read() && remaining > 0)
                    {
                        decimal batchQty = Convert.ToDecimal(reader["remaining_qty"]);
                        if (batchQty <= 0)
                        {
                            continue;
                        }

                        decimal usedQty = batchQty >= remaining ? remaining : batchQty;
                        BatchAllocationEntry entry = new BatchAllocationEntry();
                        entry.BatchId = Convert.ToInt64(reader["batch_id"]);
                        entry.Quantity = usedQty;
                        entry.UnitCost = Convert.ToDecimal(reader["unit_cost"]);
                        allocations.Add(entry);
                        remaining -= usedQty;
                    }
                }
            }

            if (remaining > 0)
            {
                throw new InvalidOperationException(string.Format("Insufficient non-expired stock for {0}.", item.ProductName));
            }

            for (int index = 0; index < allocations.Count; index++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        UPDATE product_stock_batches
                        SET remaining_qty = remaining_qty - @usedQty,
                            updated_at = NOW()
                        WHERE batch_id = @batchId;";
                    command.Parameters.AddWithValue("@usedQty", allocations[index].Quantity);
                    command.Parameters.AddWithValue("@batchId", allocations[index].BatchId);
                    command.ExecuteNonQuery();
                }
            }

            return allocations;
        }

        private static void InsertSaleBatchAllocations(MySqlConnection connection, MySqlTransaction transaction, long saleId, long saleDetailId, int productId, List<BatchAllocationEntry> allocations)
        {
            for (int index = 0; index < allocations.Count; index++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        INSERT INTO sale_batch_allocations (
                            sale_id, sale_detail_id, batch_id, product_id, quantity, unit_cost
                        )
                        VALUES (
                            @saleId, @saleDetailId, @batchId, @productId, @quantity, @unitCost
                        );";
                    command.Parameters.AddWithValue("@saleId", saleId);
                    command.Parameters.AddWithValue("@saleDetailId", saleDetailId);
                    command.Parameters.AddWithValue("@batchId", allocations[index].BatchId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@quantity", allocations[index].Quantity);
                    command.Parameters.AddWithValue("@unitCost", allocations[index].UnitCost);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void RestoreSaleBatchAllocations(MySqlConnection connection, MySqlTransaction transaction, long saleId)
        {
            List<BatchAllocationEntry> allocations = new List<BatchAllocationEntry>();

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT batch_id, quantity
                    FROM sale_batch_allocations
                    WHERE sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleId", saleId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BatchAllocationEntry entry = new BatchAllocationEntry();
                        entry.BatchId = Convert.ToInt64(reader["batch_id"]);
                        entry.Quantity = Convert.ToDecimal(reader["quantity"]);
                        allocations.Add(entry);
                    }
                }
            }

            for (int index = 0; index < allocations.Count; index++)
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = @"
                        UPDATE product_stock_batches
                        SET remaining_qty = remaining_qty + @qty,
                            status = 'Active',
                            updated_at = NOW()
                        WHERE batch_id = @batchId;";
                    command.Parameters.AddWithValue("@qty", allocations[index].Quantity);
                    command.Parameters.AddWithValue("@batchId", allocations[index].BatchId);
                    command.ExecuteNonQuery();
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM sale_batch_allocations WHERE sale_id = @saleId;";
                command.Parameters.AddWithValue("@saleId", saleId);
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
                        LookupOption option = new LookupOption();
                        option.Id = Convert.ToInt32(reader.GetValue(0));
                        option.Name = Convert.ToString(reader.GetValue(1));
                        items.Add(option);
                    }
                }
            }

            return items;
        }

        private class BatchAllocationEntry
        {
            public long BatchId { get; set; }

            public decimal Quantity { get; set; }

            public decimal UnitCost { get; set; }
        }
    }
}
