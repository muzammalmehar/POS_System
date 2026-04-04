using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopPOS.Data;
using ShopPOS.Models;

namespace ShopPOS.Services
{
    public class RecentSalesService
    {
        public List<LookupOption> GetCustomerFilters()
        {
            List<LookupOption> items = new List<LookupOption>();
            items.Add(new LookupOption { Id = 0, Name = "All Customers" });

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                SalesService.EnsureSaleTrackingColumns(connection, null);
                ServiceCenterService.EnsureServiceRefundColumns(connection);

                command.CommandText = @"
                    SELECT customer_name
                    FROM
                    (
                        SELECT DISTINCT c.customer_name
                        FROM sale_header sh
                        INNER JOIN customers c ON c.customer_id = sh.customer_id
                        WHERE IFNULL(sh.is_refunded, 0) = 0

                        UNION

                        SELECT DISTINCT sth.customer_name
                        FROM service_transaction_header sth
                        WHERE sth.customer_name IS NOT NULL
                          AND sth.customer_name <> ''
                          AND IFNULL(sth.is_refunded, 0) = 0
                    ) names
                    ORDER BY customer_name;";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    int nextId = 1;
                    while (reader.Read())
                    {
                        items.Add(new LookupOption
                        {
                            Id = nextId++,
                            Name = Convert.ToString(reader["customer_name"])
                        });
                    }
                }
            }

            return items;
        }

        public List<UnifiedRecentSaleItem> GetRecentSales(string saleTypeFilter, string customerNameFilter)
        {
            List<UnifiedRecentSaleItem> items = new List<UnifiedRecentSaleItem>();

            using (MySqlConnection connection = DatabaseConnectionFactory.CreateOpenConnection())
            using (MySqlCommand command = connection.CreateCommand())
            {
                SalesService.EnsureSaleTrackingColumns(connection, null);
                ServiceCenterService.EnsureServiceRefundColumns(connection);

                string normalizedType = string.IsNullOrWhiteSpace(saleTypeFilter) ? "All" : saleTypeFilter;
                string customerName = string.IsNullOrWhiteSpace(customerNameFilter) || customerNameFilter == "All Customers"
                    ? null
                    : customerNameFilter.Trim();

                command.CommandText = @"
                    SELECT *
                    FROM
                    (
                        SELECT
                            'Grocery' AS sale_type,
                            sh.sale_id AS record_id,
                            sh.sale_no AS document_no,
                            sh.sale_date AS transaction_date,
                            IFNULL(c.customer_name, 'Walk-in Customer') AS customer_name,
                            sh.grand_total AS gross_amount,
                            IFNULL(SUM(sd.profit_amount), 0.00) AS profit_amount,
                            sh.payment_method AS payment_info,
                            CASE WHEN IFNULL(sh.is_refunded, 0) = 1 THEN 'Refunded' ELSE 'Completed' END AS status,
                            u.full_name AS cashier_name,
                            sh.remarks
                        FROM sale_header sh
                        LEFT JOIN customers c ON c.customer_id = sh.customer_id
                        LEFT JOIN sale_detail sd ON sd.sale_id = sh.sale_id
                        INNER JOIN users u ON u.user_id = sh.created_by
                        GROUP BY
                            sh.sale_id, sh.sale_no, sh.sale_date, c.customer_name,
                            sh.grand_total, sh.payment_method, sh.is_refunded, u.full_name, sh.remarks

                        UNION ALL

                        SELECT
                            'Service' AS sale_type,
                            sth.service_txn_id AS record_id,
                            sth.txn_no AS document_no,
                            sth.txn_date AS transaction_date,
                            sth.customer_name AS customer_name,
                            sth.amount AS gross_amount,
                            sth.commission_earned AS profit_amount,
                            wa.account_name AS payment_info,
                            CASE
                                WHEN IFNULL(sth.is_refunded, 0) = 1 OR sth.status = 'Refunded' THEN 'Refunded'
                                ELSE sth.status
                            END AS status,
                            u.full_name AS cashier_name,
                            sth.remarks
                        FROM service_transaction_header sth
                        INNER JOIN wallet_accounts wa ON wa.wallet_account_id = sth.wallet_account_id
                        INNER JOIN users u ON u.user_id = sth.created_by
                    ) recent_sales
                    WHERE (@saleType = 'All' OR recent_sales.sale_type = @saleType)
                      AND (@customerName IS NULL OR recent_sales.customer_name = @customerName)
                    ORDER BY recent_sales.transaction_date DESC, recent_sales.record_id DESC
                    LIMIT 300;";

                command.Parameters.AddWithValue("@saleType", normalizedType);
                command.Parameters.AddWithValue("@customerName", (object)customerName ?? DBNull.Value);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UnifiedRecentSaleItem item = new UnifiedRecentSaleItem();
                        item.SaleType = Convert.ToString(reader["sale_type"]);
                        item.RecordId = Convert.ToInt64(reader["record_id"]);
                        item.DocumentNo = Convert.ToString(reader["document_no"]);
                        item.TransactionDate = Convert.ToDateTime(reader["transaction_date"]);
                        item.CustomerName = Convert.ToString(reader["customer_name"]);
                        item.GrossAmount = Convert.ToDecimal(reader["gross_amount"]);
                        item.ProfitAmount = Convert.ToDecimal(reader["profit_amount"]);
                        item.PaymentInfo = Convert.ToString(reader["payment_info"]);
                        item.Status = Convert.ToString(reader["status"]);
                        item.CashierName = Convert.ToString(reader["cashier_name"]);
                        item.Remarks = Convert.ToString(reader["remarks"]);
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
