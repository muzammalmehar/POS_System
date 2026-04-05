using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class ExpiryService
    {
        public List<ExpiringBatchItem> GetExpiringBatches(int daysAhead)
        {
            List<ExpiringBatchItem> items = new List<ExpiringBatchItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureExpirySchema(connection, null);
                SyncEditableBatchExpiry(connection, null, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            b.batch_id,
                            b.product_id,
                            b.supplier_id,
                            p.product_code,
                            p.product_name,
                            IFNULL(s.supplier_name, '') AS supplier_name,
                            IFNULL(b.batch_no, '') AS batch_no,
                            b.expiry_date,
                            b.remaining_qty,
                            b.unit_cost,
                            CASE
                                WHEN b.expiry_date IS NULL THEN 'No expiry date'
                                WHEN b.expiry_date < CURDATE() THEN 'Expired'
                                WHEN b.expiry_date = CURDATE() THEN 'Expires today'
                                ELSE CONCAT(DATEDIFF(b.expiry_date, CURDATE()), ' day(s) left')
                            END AS age_status
                        FROM product_stock_batches b
                        INNER JOIN products p ON p.product_id = b.product_id
                        LEFT JOIN suppliers s ON s.supplier_id = b.supplier_id
                        WHERE b.remaining_qty > 0
                          AND b.status = 'Active'
                          AND b.expiry_date IS NOT NULL
                          AND b.expiry_date <= DATE_ADD(CURDATE(), INTERVAL @daysAhead DAY)
                        ORDER BY b.expiry_date ASC, p.product_name ASC;";
                    command.Parameters.AddWithValue("@daysAhead", daysAhead);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExpiringBatchItem item = new ExpiringBatchItem();
                            item.BatchId = Convert.ToInt64(reader["batch_id"]);
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.SupplierId = reader["supplier_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["supplier_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.SupplierName = Convert.ToString(reader["supplier_name"]);
                            item.BatchNo = Convert.ToString(reader["batch_no"]);
                            item.ExpiryDate = reader["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["expiry_date"]);
                            item.RemainingQty = Convert.ToDecimal(reader["remaining_qty"]);
                            item.UnitCost = Convert.ToDecimal(reader["unit_cost"]);
                            item.AgeStatus = Convert.ToString(reader["age_status"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<ExpiredStockRecord> GetExpiredStockRecords()
        {
            List<ExpiredStockRecord> items = new List<ExpiredStockRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureExpirySchema(connection, null);
                SyncEditableBatchExpiry(connection, null, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT
                            r.expired_record_id,
                            r.product_id,
                            r.supplier_id,
                            p.product_code,
                            p.product_name,
                            IFNULL(s.supplier_name, '') AS supplier_name,
                            IFNULL(r.batch_no, '') AS batch_no,
                            r.expiry_date,
                            r.quantity,
                            r.resolution_status,
                            r.processed_at,
                            IFNULL(r.remarks, '') AS remarks
                        FROM expired_product_records r
                        INNER JOIN products p ON p.product_id = r.product_id
                        LEFT JOIN suppliers s ON s.supplier_id = r.supplier_id
                        ORDER BY r.processed_at DESC, p.product_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExpiredStockRecord item = new ExpiredStockRecord();
                            item.ExpiredRecordId = Convert.ToInt64(reader["expired_record_id"]);
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.SupplierId = reader["supplier_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["supplier_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.SupplierName = Convert.ToString(reader["supplier_name"]);
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

        public void MoveExpiredBatchToRecords(long batchId, int userId, string remarks)
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    EnsureExpirySchema(connection, transaction);

                    long productId = 0;
                    object supplierId = DBNull.Value;
                    object purchaseId = DBNull.Value;
                    decimal qty = 0;
                    decimal unitCost = 0;
                    string batchNo = null;
                    DateTime? expiryDate = null;

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            SELECT product_id, supplier_id, purchase_id, remaining_qty, unit_cost, batch_no, expiry_date
                            FROM product_stock_batches
                            WHERE batch_id = @batchId
                              AND remaining_qty > 0;";
                        command.Parameters.AddWithValue("@batchId", batchId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                throw new InvalidOperationException("Selected batch is no longer available.");
                            }

                            productId = Convert.ToInt64(reader["product_id"]);
                            supplierId = reader["supplier_id"];
                            purchaseId = reader["purchase_id"];
                            qty = Convert.ToDecimal(reader["remaining_qty"]);
                            unitCost = Convert.ToDecimal(reader["unit_cost"]);
                            batchNo = Convert.ToString(reader["batch_no"]);
                            expiryDate = reader["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["expiry_date"]);
                        }
                    }

                    if (expiryDate.HasValue && expiryDate.Value.Date > DateTime.Today)
                    {
                        throw new InvalidOperationException("This batch is not expired yet.");
                    }

                    long stockLedgerId;
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            INSERT INTO stock_ledger (
                                product_id, transaction_type, reference_id, reference_table,
                                qty_in, qty_out, unit_cost, remarks, created_by
                            )
                            VALUES (
                                @productId, 'ExpiryOut', @referenceId, 'product_stock_batches',
                                0.00, @qtyOut, @unitCost, @remarks, @createdBy
                            );
                            SELECT LAST_INSERT_ID();";
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@referenceId", batchId);
                        command.Parameters.AddWithValue("@qtyOut", qty);
                        command.Parameters.AddWithValue("@unitCost", unitCost);
                        command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)"Moved to expired stock" : remarks.Trim());
                        command.Parameters.AddWithValue("@createdBy", userId);
                        stockLedgerId = Convert.ToInt64(command.ExecuteScalar());
                    }

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            INSERT INTO expired_product_records (
                                batch_id, product_id, supplier_id, source_purchase_id, quantity, batch_no,
                                expiry_date, resolution_status, processed_at, remarks, created_by, stock_ledger_id
                            )
                            VALUES (
                                @batchId, @productId, @supplierId, @purchaseId, @quantity, @batchNo,
                                @expiryDate, 'Pending', NOW(), @remarks, @createdBy, @stockLedgerId
                            );";
                        command.Parameters.AddWithValue("@batchId", batchId);
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@supplierId", supplierId);
                        command.Parameters.AddWithValue("@purchaseId", purchaseId);
                        command.Parameters.AddWithValue("@quantity", qty);
                        command.Parameters.AddWithValue("@batchNo", string.IsNullOrWhiteSpace(batchNo) ? (object)DBNull.Value : batchNo);
                        command.Parameters.AddWithValue("@expiryDate", (object)expiryDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                        command.Parameters.AddWithValue("@createdBy", userId);
                        command.Parameters.AddWithValue("@stockLedgerId", stockLedgerId);
                        command.ExecuteNonQuery();
                    }

                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                            UPDATE product_stock_batches
                            SET remaining_qty = 0,
                                status = 'Expired',
                                updated_at = NOW()
                            WHERE batch_id = @batchId;";
                        command.Parameters.AddWithValue("@batchId", batchId);
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

        public void UpdateExpiredRecordResolution(long expiredRecordId, string resolutionStatus, string remarks)
        {
            if (string.IsNullOrWhiteSpace(resolutionStatus))
            {
                throw new InvalidOperationException("Select a resolution status.");
            }

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureExpirySchema(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE expired_product_records
                        SET resolution_status = @resolutionStatus,
                            resolution_date = NOW(),
                            remarks = @remarks
                        WHERE expired_record_id = @expiredRecordId;";
                    command.Parameters.AddWithValue("@resolutionStatus", resolutionStatus.Trim());
                    command.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? (object)DBNull.Value : remarks.Trim());
                    command.Parameters.AddWithValue("@expiredRecordId", expiredRecordId);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void EnsureExpirySchema(MySqlConnection connection, MySqlTransaction transaction)
        {
            EnsureColumn(connection, transaction, "products", "track_expiry", "ALTER TABLE products ADD COLUMN track_expiry TINYINT(1) NOT NULL DEFAULT 0 AFTER track_stock;");
            EnsureColumn(connection, transaction, "products", "default_shelf_life_days", "ALTER TABLE products ADD COLUMN default_shelf_life_days INT NULL AFTER track_expiry;");
            EnsureColumn(connection, transaction, "products", "default_expiry_date", "ALTER TABLE products ADD COLUMN default_expiry_date DATE NULL AFTER default_shelf_life_days;");
            EnsureColumn(connection, transaction, "purchase_detail", "batch_no", "ALTER TABLE purchase_detail ADD COLUMN batch_no VARCHAR(100) NULL AFTER line_total;");
            EnsureColumn(connection, transaction, "purchase_detail", "expiry_date", "ALTER TABLE purchase_detail ADD COLUMN expiry_date DATE NULL AFTER batch_no;");

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS product_stock_batches (
                        batch_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        product_id INT NOT NULL,
                        supplier_id INT NULL,
                        purchase_id BIGINT NULL,
                        purchase_detail_id BIGINT NULL,
                        batch_no VARCHAR(100) NULL,
                        expiry_date DATE NULL,
                        received_qty DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        remaining_qty DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        unit_cost DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        status VARCHAR(30) NOT NULL DEFAULT 'Active',
                        received_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        updated_at DATETIME NULL,
                        KEY idx_product_batches_product (product_id),
                        KEY idx_product_batches_expiry (expiry_date),
                        CONSTRAINT fk_product_batches_product
                            FOREIGN KEY (product_id) REFERENCES products(product_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_product_batches_supplier
                            FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id)
                            ON DELETE SET NULL ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS sale_batch_allocations (
                        allocation_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        sale_id BIGINT NOT NULL,
                        sale_detail_id BIGINT NOT NULL,
                        batch_id BIGINT NOT NULL,
                        product_id INT NOT NULL,
                        quantity DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        unit_cost DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        KEY idx_sale_allocations_sale (sale_id),
                        KEY idx_sale_allocations_batch (batch_id),
                        CONSTRAINT fk_sale_allocations_batch
                            FOREIGN KEY (batch_id) REFERENCES product_stock_batches(batch_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_sale_allocations_product
                            FOREIGN KEY (product_id) REFERENCES products(product_id)
                            ON DELETE CASCADE ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS expired_product_records (
                        expired_record_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                        batch_id BIGINT NOT NULL,
                        product_id INT NOT NULL,
                        supplier_id INT NULL,
                        source_purchase_id BIGINT NULL,
                        quantity DECIMAL(18,2) NOT NULL DEFAULT 0.00,
                        batch_no VARCHAR(100) NULL,
                        expiry_date DATE NULL,
                        resolution_status VARCHAR(30) NOT NULL DEFAULT 'Pending',
                        processed_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        resolution_date DATETIME NULL,
                        remarks VARCHAR(255) NULL,
                        created_by INT NOT NULL,
                        stock_ledger_id BIGINT NULL,
                        KEY idx_expired_records_status (resolution_status),
                        CONSTRAINT fk_expired_records_batch
                            FOREIGN KEY (batch_id) REFERENCES product_stock_batches(batch_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_expired_records_product
                            FOREIGN KEY (product_id) REFERENCES products(product_id)
                            ON DELETE CASCADE ON UPDATE CASCADE,
                        CONSTRAINT fk_expired_records_supplier
                            FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id)
                            ON DELETE SET NULL ON UPDATE CASCADE
                    ) ENGINE=InnoDB;";
                command.ExecuteNonQuery();
            }
        }

        internal static void SyncEditableBatchExpiry(MySqlConnection connection, MySqlTransaction transaction, int? productId)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = @"
                    UPDATE product_stock_batches b
                    INNER JOIN products p ON p.product_id = b.product_id
                    SET
                        b.expiry_date = p.default_expiry_date,
                        b.status = CASE
                            WHEN b.remaining_qty <= 0 THEN 'Consumed'
                            WHEN p.default_expiry_date IS NOT NULL AND p.default_expiry_date < CURDATE() THEN 'Expired'
                            ELSE 'Active'
                        END,
                        b.updated_at = NOW()
                    WHERE IFNULL(p.track_expiry, 0) = 1
                      AND b.remaining_qty > 0
                      AND (@productId IS NULL OR b.product_id = @productId)
                      AND b.purchase_id IS NULL
                      AND b.purchase_detail_id IS NULL
                      AND
                      (
                          b.batch_no IS NULL OR
                          b.batch_no LIKE 'LEGACY-%' OR
                          b.batch_no LIKE 'OPEN-%' OR
                          b.batch_no LIKE 'ADJ-%'
                      )
                      AND
                      (
                          NOT (b.expiry_date <=> p.default_expiry_date) OR
                          (b.status = 'Expired' AND (p.default_expiry_date IS NULL OR p.default_expiry_date >= CURDATE())) OR
                          (b.status = 'Active' AND p.default_expiry_date IS NOT NULL AND p.default_expiry_date < CURDATE())
                      );";
                command.Parameters.AddWithValue("@productId", (object)productId ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
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
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
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
    }
}
