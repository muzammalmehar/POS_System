using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class StockService
    {
        public List<StockOverviewItem> GetStockOverview()
        {
            List<StockOverviewItem> items = new List<StockOverviewItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                ExpiryService.EnsureExpirySchema(connection, null);
                ExpiryService.SyncEditableBatchExpiry(connection, null, null);
                EnsureLegacyExpiryTrackedBatches(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            p.product_id,
                            p.product_code,
                            p.product_name,
                            u.short_name,
                            p.purchase_price,
                            p.sale_price,
                            p.reorder_level,
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
                            p.product_id, p.product_code, p.product_name,
                            u.short_name, p.purchase_price, p.sale_price, p.reorder_level, p.track_expiry
                        ORDER BY p.product_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StockOverviewItem item = new StockOverviewItem();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.UnitName = Convert.ToString(reader["short_name"]);
                            item.PurchasePrice = Convert.ToDecimal(reader["purchase_price"]);
                            item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                            item.ReorderLevel = Convert.ToDecimal(reader["reorder_level"]);
                            item.CurrentStock = Convert.ToDecimal(reader["current_stock"]);
                            item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<StockMovementItem> GetRecentMovements(int productId)
        {
            List<StockMovementItem> items = new List<StockMovementItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        sl.created_at,
                        sl.transaction_type,
                        sl.qty_in,
                        sl.qty_out,
                        sl.unit_cost,
                        sl.remarks,
                        IFNULL(u.full_name, 'System') AS created_by_name
                    FROM stock_ledger sl
                    LEFT JOIN users u ON u.user_id = sl.created_by
                    WHERE sl.product_id = @productId
                    ORDER BY sl.created_at DESC
                    LIMIT 20;";
                command.Parameters.AddWithValue("@productId", productId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StockMovementItem item = new StockMovementItem();
                        item.CreatedAt = Convert.ToDateTime(reader["created_at"]);
                        item.TransactionType = Convert.ToString(reader["transaction_type"]);
                        item.QtyIn = Convert.ToDecimal(reader["qty_in"]);
                        item.QtyOut = Convert.ToDecimal(reader["qty_out"]);
                        item.UnitCost = Convert.ToDecimal(reader["unit_cost"]);
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        item.CreatedByName = Convert.ToString(reader["created_by_name"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public void SaveAdjustment(StockAdjustmentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.ProductId <= 0)
            {
                throw new InvalidOperationException("Select a product first.");
            }

            if (request.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantity must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.TransactionType))
            {
                throw new InvalidOperationException("Select an adjustment type.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    ExpiryService.EnsureExpirySchema(connection, transaction);
                    ProductStockContext context = GetProductContext(connection, transaction, request.ProductId);
                    EnsureLegacyExpiryTrackedBatches(connection, transaction, request.ProductId);

                    decimal currentStock = context.TrackExpiry
                        ? GetAvailableBatchStock(connection, transaction, request.ProductId)
                        : GetLedgerCurrentStock(connection, transaction, request.ProductId);

                    if ((request.TransactionType == "StockAdjustOut" || request.TransactionType == "Damage")
                        && currentStock < request.Quantity)
                    {
                        throw new InvalidOperationException(
                            string.Format("Not enough stock for this adjustment. Available stock: {0:N2}", currentStock));
                    }

                    decimal qtyIn = request.TransactionType == "OpeningStock" || request.TransactionType == "StockAdjustIn"
                        ? request.Quantity
                        : 0;
                    decimal qtyOut = request.TransactionType == "StockAdjustOut" || request.TransactionType == "Damage"
                        ? request.Quantity
                        : 0;

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            INSERT INTO stock_ledger (
                                product_id, transaction_type, reference_id, reference_table,
                                qty_in, qty_out, unit_cost, remarks, created_by
                            )
                            VALUES (
                                @productId, @transactionType, NULL, 'manual_adjustment',
                                @qtyIn, @qtyOut, @unitCost, @remarks, @createdBy
                            );";

                        command.Parameters.AddWithValue("@productId", request.ProductId);
                        command.Parameters.AddWithValue("@transactionType", request.TransactionType);
                        command.Parameters.AddWithValue("@qtyIn", qtyIn);
                        command.Parameters.AddWithValue("@qtyOut", qtyOut);
                        command.Parameters.AddWithValue("@unitCost", request.UnitCost);
                        command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(request.Remarks) ? (object)DBNull.Value : request.Remarks.Trim());
                        command.Parameters.AddWithValue("@createdBy", request.UserId);
                        command.ExecuteNonQuery();
                    }

                    if (context.TrackExpiry)
                    {
                        if (qtyIn > 0)
                        {
                            InsertManualBatchRecord(connection, transaction, context, request, qtyIn);
                        }
                        else if (qtyOut > 0)
                        {
                            ConsumeManualBatchStock(connection, transaction, request.ProductId, request.Quantity);
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

        private static ProductStockContext GetProductContext(MySqlConnection connection, MySqlTransaction transaction, int productId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT
                        product_id,
                        product_name,
                        IFNULL(track_expiry, 0) AS track_expiry,
                        purchase_price,
                        default_expiry_date
                    FROM products
                    WHERE product_id = @productId;";
                command.Parameters.AddWithValue("@productId", productId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("Selected product was not found.");
                    }

                    ProductStockContext context = new ProductStockContext();
                    context.ProductId = Convert.ToInt32(reader["product_id"]);
                    context.ProductName = Convert.ToString(reader["product_name"]);
                    context.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                    context.PurchasePrice = Convert.ToDecimal(reader["purchase_price"]);
                    context.DefaultExpiryDate = reader["default_expiry_date"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(reader["default_expiry_date"]);
                    return context;
                }
            }
        }

        private static decimal GetLedgerCurrentStock(MySqlConnection connection, MySqlTransaction transaction, int productId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT IFNULL(SUM(qty_in - qty_out), 0.00)
                    FROM stock_ledger
                    WHERE product_id = @productId;";
                command.Parameters.AddWithValue("@productId", productId);
                return Convert.ToDecimal(command.ExecuteScalar());
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

        private static void InsertManualBatchRecord(MySqlConnection connection, MySqlTransaction transaction, ProductStockContext context, StockAdjustmentRequest request, decimal quantity)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    INSERT INTO product_stock_batches (
                        product_id, supplier_id, purchase_id, purchase_detail_id, batch_no, expiry_date,
                        received_qty, remaining_qty, unit_cost, status, received_at, updated_at
                    )
                    VALUES (
                        @productId, NULL, NULL, NULL, @batchNo, @expiryDate,
                        @receivedQty, @remainingQty, @unitCost, 'Active', NOW(), NOW()
                    );";
                command.Parameters.AddWithValue("@productId", context.ProductId);
                command.Parameters.AddWithValue("@batchNo", BuildManualBatchNo(request.TransactionType, context.ProductId));
                command.Parameters.AddWithValue("@expiryDate", (object)context.DefaultExpiryDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@receivedQty", quantity);
                command.Parameters.AddWithValue("@remainingQty", quantity);
                command.Parameters.AddWithValue("@unitCost", request.UnitCost > 0 ? request.UnitCost : context.PurchasePrice);
                command.ExecuteNonQuery();
            }
        }

        private static void ConsumeManualBatchStock(MySqlConnection connection, MySqlTransaction transaction, int productId, decimal quantity)
        {
            decimal remaining = quantity;

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT batch_id, remaining_qty
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
                command.Parameters.AddWithValue("@productId", productId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    List<BatchConsumptionEntry> batches = new List<BatchConsumptionEntry>();
                    while (reader.Read() && remaining > 0)
                    {
                        decimal batchQty = Convert.ToDecimal(reader["remaining_qty"]);
                        if (batchQty <= 0)
                        {
                            continue;
                        }

                        decimal usedQty = batchQty >= remaining ? remaining : batchQty;
                        BatchConsumptionEntry entry = new BatchConsumptionEntry();
                        entry.BatchId = Convert.ToInt64(reader["batch_id"]);
                        entry.Quantity = usedQty;
                        batches.Add(entry);
                        remaining -= usedQty;
                    }

                    if (remaining > 0)
                    {
                        throw new InvalidOperationException("Batch stock could not be synchronized for the selected product.");
                    }

                    reader.Close();

                    for (int index = 0; index < batches.Count; index++)
                    {
                        using (MySqlCommand update = connection.CreateCommand())
                        {
                            update.Transaction = transaction;
                            update.CommandText = @"
                                UPDATE product_stock_batches
                                SET remaining_qty = remaining_qty - @usedQty,
                                    status = CASE WHEN remaining_qty - @usedQty <= 0 THEN 'Consumed' ELSE status END,
                                    updated_at = NOW()
                                WHERE batch_id = @batchId;";
                            update.Parameters.AddWithValue("@usedQty", batches[index].Quantity);
                            update.Parameters.AddWithValue("@batchId", batches[index].BatchId);
                            update.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void EnsureLegacyExpiryTrackedBatches(MySqlConnection connection, MySqlTransaction transaction)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT p.product_id
                    FROM products p
                    WHERE IFNULL(p.track_expiry, 0) = 1
                      AND p.is_active = 1
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM product_stock_batches b
                          WHERE b.product_id = p.product_id
                            AND b.remaining_qty > 0
                      );";

                List<int> productIds = new List<int>();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productIds.Add(Convert.ToInt32(reader["product_id"]));
                    }
                }

                for (int index = 0; index < productIds.Count; index++)
                {
                    EnsureLegacyExpiryTrackedBatches(connection, transaction, productIds[index]);
                }
            }
        }

        private static void EnsureLegacyExpiryTrackedBatches(MySqlConnection connection, MySqlTransaction transaction, int productId)
        {
            ProductStockContext context = GetProductContext(connection, transaction, productId);
            if (!context.TrackExpiry)
            {
                return;
            }

            decimal positiveBatchCount = GetPositiveBatchRowCount(connection, transaction, productId);
            if (positiveBatchCount > 0)
            {
                return;
            }

            decimal ledgerStock = GetLedgerCurrentStock(connection, transaction, productId);
            if (ledgerStock <= 0)
            {
                return;
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
                        @productId, NULL, NULL, NULL, @batchNo, @expiryDate,
                        @receivedQty, @remainingQty, @unitCost, 'Active', NOW(), NOW()
                    );";
                command.Parameters.AddWithValue("@productId", productId);
                command.Parameters.AddWithValue("@batchNo", BuildLegacyBatchNo(productId));
                command.Parameters.AddWithValue("@expiryDate", (object)context.DefaultExpiryDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@receivedQty", ledgerStock);
                command.Parameters.AddWithValue("@remainingQty", ledgerStock);
                command.Parameters.AddWithValue("@unitCost", context.PurchasePrice);
                command.ExecuteNonQuery();
            }
        }

        private static int GetPositiveBatchRowCount(MySqlConnection connection, MySqlTransaction transaction, int productId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM product_stock_batches
                    WHERE product_id = @productId
                      AND remaining_qty > 0;";
                command.Parameters.AddWithValue("@productId", productId);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static string BuildManualBatchNo(string transactionType, int productId)
        {
            return string.Format(
                "{0}-{1}-{2}",
                transactionType == "OpeningStock" ? "OPEN" : "ADJ",
                productId,
                DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        private static string BuildLegacyBatchNo(int productId)
        {
            return string.Format("LEGACY-{0}-{1}", productId, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        private sealed class ProductStockContext
        {
            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public bool TrackExpiry { get; set; }

            public decimal PurchasePrice { get; set; }

            public DateTime? DefaultExpiryDate { get; set; }
        }

        private sealed class BatchConsumptionEntry
        {
            public long BatchId { get; set; }

            public decimal Quantity { get; set; }
        }
    }
}
