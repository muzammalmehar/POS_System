using System;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class DashboardService
    {
        public DashboardMetrics GetMetrics()
        {
            DashboardMetrics metrics = new DashboardMetrics();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            {
                SalesService.EnsureSaleTrackingColumns(connection, null);
                ServiceCenterService.EnsureServiceRefundColumns(connection);
                ExpiryService.EnsureExpirySchema(connection, null);
                LoadTodaySales(metrics, connection);
                LoadTodayExpenses(metrics, connection);
                LoadTodayServices(metrics, connection);
                LoadLowStockItems(metrics, connection);
                LoadExpiryAlerts(metrics, connection);
                LoadRecentSales(metrics, connection);
                LoadStockValueEstimate(metrics, connection);
            }

            return metrics;
        }

        private static void LoadTodaySales(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        IFNULL(SUM(sale_summary.grand_total), 0.00) AS today_sales,
                        IFNULL(SUM(sale_summary.grand_total - sale_summary.total_cost), 0.00) AS today_profit,
                        IFNULL(SUM(CASE
                            WHEN sale_summary.payment_method = 'Credit'
                                 OR sale_summary.paid_amount < sale_summary.grand_total
                            THEN sale_summary.grand_total
                            ELSE 0.00
                        END), 0.00) AS today_credit_sales,
                        COUNT(*) AS total_sales
                    FROM
                    (
                        SELECT
                            sh.sale_id,
                            sh.grand_total,
                            sh.paid_amount,
                            sh.payment_method,
                            IFNULL(SUM(sd.cost_rate * sd.quantity), 0.00) AS total_cost
                        FROM sale_header sh
                        LEFT JOIN sale_detail sd ON sd.sale_id = sh.sale_id
                        WHERE DATE(sh.sale_date) = CURDATE()
                          AND IFNULL(sh.is_refunded, 0) = 0
                        GROUP BY sh.sale_id, sh.grand_total, sh.paid_amount, sh.payment_method
                    ) AS sale_summary;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return;
                    }

                    metrics.TodaySalesAmount = Convert.ToDecimal(reader["today_sales"]);
                    metrics.TodaySalesProfit = Convert.ToDecimal(reader["today_profit"]);
                    metrics.TodayCreditSalesAmount = Convert.ToDecimal(reader["today_credit_sales"]);
                    metrics.TodaySalesCount = Convert.ToInt32(reader["total_sales"]);
                }
            }
        }

        private static void LoadTodayExpenses(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT IFNULL(SUM(amount), 0.00)
                    FROM expenses
                    WHERE DATE(expense_date) = CURDATE();";

                metrics.TodayExpensesAmount = Convert.ToDecimal(command.ExecuteScalar());
            }
        }

        private static void LoadTodayServices(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT
                        IFNULL(SUM(amount), 0.00) AS service_amount,
                        IFNULL(SUM(commission_earned), 0.00) AS service_profit
                    FROM service_transaction_header
                    WHERE DATE(txn_date) = CURDATE()
                      AND status = 'Completed';";
                      

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return;
                    }

                    metrics.TodayServiceAmount = Convert.ToDecimal(reader["service_amount"]);
                    metrics.TodayServiceIncome = Convert.ToDecimal(reader["service_profit"]);
                }
            }
        }

        private static void LoadLowStockItems(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM (
                        SELECT p.product_id
                        FROM products p
                        LEFT JOIN stock_ledger sl ON sl.product_id = p.product_id
                        WHERE p.track_stock = 1
                          AND p.is_active = 1
                        GROUP BY p.product_id, p.reorder_level
                        HAVING IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00) <= p.reorder_level
                    ) AS low_stock_products;";

                metrics.LowStockCount = Convert.ToInt32(command.ExecuteScalar());
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT p.product_code,
                           p.product_name,
                           IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00) AS current_stock,
                           p.reorder_level
                    FROM products p
                    LEFT JOIN stock_ledger sl ON sl.product_id = p.product_id
                    WHERE p.track_stock = 1
                      AND p.is_active = 1
                    GROUP BY p.product_id, p.product_code, p.product_name, p.reorder_level
                    HAVING IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00) <= p.reorder_level
                    ORDER BY current_stock ASC, p.product_name ASC
                    LIMIT 10;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LowStockItem item = new LowStockItem();
                        item.ProductCode = Convert.ToString(reader["product_code"]);
                        item.ProductName = Convert.ToString(reader["product_name"]);
                        item.CurrentStock = Convert.ToDecimal(reader["current_stock"]);
                        item.ReorderLevel = Convert.ToDecimal(reader["reorder_level"]);
                        metrics.LowStockItems.Add(item);
                    }
                }
            }
        }

        private static void LoadRecentSales(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT recent_entries.entry_type,
                           recent_entries.sale_no,
                           recent_entries.sale_date,
                           recent_entries.grand_total,
                           recent_entries.payment_method,
                           recent_entries.cashier
                    FROM
                    (
                        SELECT
                            'Grocery' AS entry_type,
                            sh.sale_no,
                            sh.sale_date,
                            sh.grand_total,
                            COALESCE(NULLIF(TRIM(sh.payment_method), ''), 'Cash') AS payment_method,
                            COALESCE(NULLIF(TRIM(u.full_name), ''), 'System') AS cashier
                        FROM sale_header sh
                        LEFT JOIN users u ON u.user_id = sh.created_by
                        WHERE IFNULL(sh.is_refunded, 0) = 0

                        UNION ALL

                        SELECT
                            'Service' AS entry_type,
                            COALESCE(NULLIF(TRIM(sth.txn_no), ''), CONCAT('SRV-', sth.service_txn_id)) AS sale_no,
                            sth.txn_date AS sale_date,
                            sth.amount AS grand_total,
                            COALESCE(NULLIF(TRIM(sth.payment_method), ''), 'N/A') AS payment_method,
                            COALESCE(NULLIF(TRIM(u.full_name), ''), 'System') AS cashier
                        FROM service_transaction_header sth
                        LEFT JOIN users u ON u.user_id = sth.created_by
                        WHERE IFNULL(sth.is_refunded, 0) = 0
                          AND sth.status = 'Completed'
                    ) AS recent_entries
                    ORDER BY recent_entries.sale_date DESC
                    LIMIT 8;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RecentSaleItem item = new RecentSaleItem();
                        item.EntryType = Convert.ToString(reader["entry_type"]);
                        item.SaleNo = Convert.ToString(reader["sale_no"]);
                        item.SaleDate = Convert.ToDateTime(reader["sale_date"]);
                        item.GrandTotal = Convert.ToDecimal(reader["grand_total"]);
                        item.PaymentMethod = Convert.ToString(reader["payment_method"]);
                        item.Cashier = Convert.ToString(reader["cashier"]);
                        metrics.RecentSales.Add(item);
                    }
                }
            }
        }

        private static void LoadExpiryAlerts(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM product_stock_batches
                    WHERE remaining_qty > 0
                      AND status = 'Active'
                      AND expiry_date IS NOT NULL
                      AND expiry_date <= DATE_ADD(CURDATE(), INTERVAL 30 DAY);";
                metrics.ExpiryAlertCount = Convert.ToInt32(command.ExecuteScalar());
            }

            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT COUNT(*)
                    FROM expired_product_records
                    WHERE resolution_status = 'Pending';";
                metrics.ExpiredPendingCount = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static void LoadStockValueEstimate(DashboardMetrics metrics, MySqlConnection connection)
        {
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT IFNULL(SUM(stock_summary.current_stock * stock_summary.purchase_price), 0.00)
                    FROM (
                        SELECT p.product_id,
                               p.purchase_price,
                               IFNULL(SUM(sl.qty_in - sl.qty_out), 0.00) AS current_stock
                        FROM products p
                        LEFT JOIN stock_ledger sl ON sl.product_id = p.product_id
                        WHERE p.track_stock = 1
                          AND p.is_active = 1
                        GROUP BY p.product_id, p.purchase_price
                    ) AS stock_summary;";

                metrics.StockValueEstimate = Convert.ToDecimal(command.ExecuteScalar());
            }
        }
    }
}
