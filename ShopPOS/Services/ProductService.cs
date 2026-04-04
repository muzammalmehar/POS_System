using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class ProductService
    {
        public List<ProductRecord> GetProducts()
        {
            List<ProductRecord> items = new List<ProductRecord>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureImageColumn(connection);
                ExpiryService.EnsureExpirySchema(connection, null);

                using (MySqlCommand command = connection.CreateCommand())
                {
                    EnsureVendorEnhancements(connection);

                    command.CommandText = @"
                    SELECT
                        p.product_id,
                        p.product_code,
                        p.barcode,
                        p.product_name,
                        p.category_id,
                        c.category_name,
                        p.brand_id,
                        IFNULL(b.brand_name, '') AS brand_name,
                        p.base_unit_id,
                        u.short_name,
                        p.purchase_price,
                        p.sale_price,
                        p.reorder_level,
                        p.track_stock,
                        p.track_expiry,
                        p.default_shelf_life_days,
                        p.default_expiry_date,
                        p.is_active,
                        p.image_path,
                        s.supplier_id AS preferred_vendor_id,
                        IFNULL(s.supplier_name, '') AS preferred_vendor_name
                    FROM products p
                    INNER JOIN categories c ON c.category_id = p.category_id
                    INNER JOIN units u ON u.unit_id = p.base_unit_id
                    LEFT JOIN brands b ON b.brand_id = p.brand_id
                    LEFT JOIN supplier_products sp
                        ON sp.product_id = p.product_id
                       AND sp.is_preferred = 1
                    LEFT JOIN suppliers s ON s.supplier_id = sp.supplier_id
                    ORDER BY p.product_name ASC;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductRecord item = new ProductRecord();
                            item.ProductId = Convert.ToInt32(reader["product_id"]);
                            item.ProductCode = Convert.ToString(reader["product_code"]);
                            item.Barcode = Convert.ToString(reader["barcode"]);
                            item.ProductName = Convert.ToString(reader["product_name"]);
                            item.CategoryId = Convert.ToInt32(reader["category_id"]);
                            item.CategoryName = Convert.ToString(reader["category_name"]);
                            item.BrandId = reader["brand_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["brand_id"]);
                            item.BrandName = Convert.ToString(reader["brand_name"]);
                            item.BaseUnitId = Convert.ToInt32(reader["base_unit_id"]);
                            item.UnitName = Convert.ToString(reader["short_name"]);
                            item.PurchasePrice = Convert.ToDecimal(reader["purchase_price"]);
                            item.SalePrice = Convert.ToDecimal(reader["sale_price"]);
                            item.ReorderLevel = Convert.ToDecimal(reader["reorder_level"]);
                            item.TrackStock = Convert.ToBoolean(reader["track_stock"]);
                            item.TrackExpiry = Convert.ToBoolean(reader["track_expiry"]);
                            item.DefaultShelfLifeDays = reader["default_shelf_life_days"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["default_shelf_life_days"]);
                            item.DefaultExpiryDate = reader["default_expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["default_expiry_date"]);
                            item.IsActive = Convert.ToBoolean(reader["is_active"]);
                            item.ImagePath = Convert.ToString(reader["image_path"]);
                            item.PreferredVendorId = reader["preferred_vendor_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["preferred_vendor_id"]);
                            item.PreferredVendorName = Convert.ToString(reader["preferred_vendor_name"]);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }

        public List<LookupOption> GetCategories()
        {
            return LoadLookupOptions("SELECT category_id, category_name FROM categories WHERE is_active = 1 ORDER BY category_name ASC;");
        }

        public List<LookupOption> GetBrands()
        {
            List<LookupOption> items = LoadLookupOptions("SELECT brand_id, brand_name FROM brands WHERE is_active = 1 ORDER BY brand_name ASC;");
            items.Insert(0, new LookupOption { Id = 0, Name = "No Brand" });
            return items;
        }

        public List<LookupOption> GetUnits()
        {
            return LoadLookupOptions("SELECT unit_id, short_name FROM units ORDER BY unit_name ASC;");
        }

        public List<LookupOption> GetVendors()
        {
            List<LookupOption> items = LoadLookupOptions("SELECT supplier_id, supplier_name FROM suppliers WHERE is_active = 1 ORDER BY supplier_name ASC;");
            items.Insert(0, new LookupOption { Id = 0, Name = "No Vendor Selected" });
            return items;
        }

        public string GenerateNextProductCode()
        {
            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT MAX(CAST(SUBSTRING(product_code, 5) AS UNSIGNED))
                    FROM products
                    WHERE product_code LIKE 'PRD-%';";

                object value = command.ExecuteScalar();
                int nextNumber = 1;

                if (!(value is DBNull) && value != null)
                {
                    nextNumber = Convert.ToInt32(value) + 1;
                }

                return string.Format("PRD-{0:0000}", nextNumber);
            }
        }

        public void SaveProduct(ProductSaveRequest request)
        {
            ValidateRequest(request);

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                EnsureImageColumn(connection);
                ExpiryService.EnsureExpirySchema(connection, null);
                EnsureVendorEnhancements(connection);
                EnsureUniqueProductCode(connection, request);

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int productId;
                        using (MySqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            if (request.ProductId.HasValue)
                            {
                                command.CommandText = @"
                                    UPDATE products
                                    SET product_code = @productCode,
                                        barcode = @barcode,
                                        product_name = @productName,
                                        category_id = @categoryId,
                                        brand_id = @brandId,
                                        base_unit_id = @baseUnitId,
                                        purchase_price = @purchasePrice,
                                        sale_price = @salePrice,
                                        reorder_level = @reorderLevel,
                                        track_stock = @trackStock,
                                        track_expiry = @trackExpiry,
                                        default_shelf_life_days = @defaultShelfLifeDays,
                                        default_expiry_date = @defaultExpiryDate,
                                        is_active = @isActive,
                                        image_path = @imagePath,
                                        updated_at = NOW()
                                    WHERE product_id = @productId;";
                                command.Parameters.AddWithValue("@productId", request.ProductId.Value);
                                productId = request.ProductId.Value;
                            }
                            else
                            {
                                command.CommandText = @"
                                    INSERT INTO products (
                                        product_code, barcode, product_name, category_id, brand_id, base_unit_id,
                                        purchase_price, sale_price, reorder_level, track_stock, track_expiry, default_shelf_life_days, default_expiry_date, is_packet_product,
                                        pieces_per_packet, packet_purchase_price, packet_sale_price, piece_sale_price, image_path, is_active
                                    )
                                    VALUES (
                                        @productCode, @barcode, @productName, @categoryId, @brandId, @baseUnitId,
                                        @purchasePrice, @salePrice, @reorderLevel, @trackStock, @trackExpiry, @defaultShelfLifeDays, @defaultExpiryDate, 0,
                                        NULL, NULL, NULL, NULL, @imagePath, @isActive
                                    );";
                                productId = 0;
                            }

                            command.Parameters.AddWithValue("@productCode", request.ProductCode.Trim());
                            command.Parameters.AddWithValue("@barcode", string.IsNullOrWhiteSpace(request.Barcode) ? (object)DBNull.Value : request.Barcode.Trim());
                            command.Parameters.AddWithValue("@productName", request.ProductName.Trim());
                            command.Parameters.AddWithValue("@categoryId", request.CategoryId);
                            command.Parameters.AddWithValue("@brandId", (object)request.BrandId ?? DBNull.Value);
                            command.Parameters.AddWithValue("@baseUnitId", request.BaseUnitId);
                            command.Parameters.AddWithValue("@purchasePrice", request.PurchasePrice);
                            command.Parameters.AddWithValue("@salePrice", request.SalePrice);
                            command.Parameters.AddWithValue("@reorderLevel", request.ReorderLevel);
                            command.Parameters.AddWithValue("@trackStock", request.TrackStock);
                            command.Parameters.AddWithValue("@trackExpiry", request.TrackExpiry);
                            command.Parameters.AddWithValue("@defaultShelfLifeDays", (object)request.DefaultShelfLifeDays ?? DBNull.Value);
                            command.Parameters.AddWithValue("@defaultExpiryDate", (object)request.DefaultExpiryDate ?? DBNull.Value);
                            command.Parameters.AddWithValue("@imagePath", string.IsNullOrWhiteSpace(request.ImagePath) ? (object)DBNull.Value : request.ImagePath.Trim());
                            command.Parameters.AddWithValue("@isActive", request.IsActive);
                            command.ExecuteNonQuery();

                            if (!request.ProductId.HasValue)
                            {
                                productId = Convert.ToInt32(command.LastInsertedId);
                            }
                        }

                        SavePreferredVendorLink(connection, transaction, productId, request.PreferredVendorId, request.PurchasePrice);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void SavePreferredVendorLink(MySqlConnection connection, MySqlTransaction transaction, int productId, int? preferredVendorId, decimal purchasePrice)
        {
            using (MySqlCommand clearCommand = connection.CreateCommand())
            {
                clearCommand.Transaction = transaction;
                clearCommand.CommandText = "UPDATE supplier_products SET is_preferred = 0 WHERE product_id = @productId;";
                clearCommand.Parameters.AddWithValue("@productId", productId);
                clearCommand.ExecuteNonQuery();
            }

            if (!preferredVendorId.HasValue || preferredVendorId.Value <= 0)
            {
                return;
            }

            using (MySqlCommand linkCommand = connection.CreateCommand())
            {
                linkCommand.Transaction = transaction;
                linkCommand.CommandText = @"
                    INSERT INTO supplier_products (
                        supplier_id, product_id, last_purchase_price, is_preferred
                    )
                    VALUES (
                        @supplierId, @productId, @lastPurchasePrice, 1
                    )
                    ON DUPLICATE KEY UPDATE
                        last_purchase_price = VALUES(last_purchase_price),
                        is_preferred = 1;";
                linkCommand.Parameters.AddWithValue("@supplierId", preferredVendorId.Value);
                linkCommand.Parameters.AddWithValue("@productId", productId);
                linkCommand.Parameters.AddWithValue("@lastPurchasePrice", purchasePrice);
                linkCommand.ExecuteNonQuery();
            }
        }

        private static void EnsureImageColumn(MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'products'
                      AND COLUMN_NAME = 'image_path';";

                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    return;
                }
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "ALTER TABLE products ADD COLUMN image_path VARCHAR(255) NULL AFTER piece_sale_price;";
                command.ExecuteNonQuery();
            }
        }

        private static void EnsureVendorEnhancements(MySqlConnection connection)
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

        private static void ValidateRequest(ProductSaveRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrWhiteSpace(request.ProductCode))
            {
                throw new InvalidOperationException("Product code is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                throw new InvalidOperationException("Product name is required.");
            }

            if (request.CategoryId <= 0)
            {
                throw new InvalidOperationException("Category is required.");
            }

            if (request.BaseUnitId <= 0)
            {
                throw new InvalidOperationException("Base unit is required.");
            }

            if (request.TrackExpiry && request.DefaultShelfLifeDays.HasValue && request.DefaultShelfLifeDays.Value <= 0)
            {
                throw new InvalidOperationException("Default shelf life must be greater than zero.");
            }
        }

        private static void EnsureUniqueProductCode(MySqlConnection connection, ProductSaveRequest request)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM products
                    WHERE product_code = @productCode
                      AND (@productId IS NULL OR product_id <> @productId);";

                command.Parameters.AddWithValue("@productCode", request.ProductCode.Trim());
                command.Parameters.AddWithValue("@productId", (object)request.ProductId ?? DBNull.Value);

                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count > 0)
                {
                    throw new InvalidOperationException("This product code already exists.");
                }
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
    }
}
