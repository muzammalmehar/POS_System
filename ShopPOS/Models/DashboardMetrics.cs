using System.Collections.Generic;

namespace ShopPOS.Models
{
    public class DashboardMetrics
    {
        public DashboardMetrics()
        {
            LowStockItems = new List<LowStockItem>();
            RecentSales = new List<RecentSaleItem>();
        }

        public decimal TodaySalesAmount { get; set; }

        public decimal TodaySalesProfit { get; set; }

        public int TodaySalesCount { get; set; }

        public decimal TodayExpensesAmount { get; set; }

        public decimal TodayCreditSalesAmount { get; set; }

        public decimal TodayServiceAmount { get; set; }

        public decimal TodayServiceIncome { get; set; }

        public int LowStockCount { get; set; }

        public int ExpiryAlertCount { get; set; }

        public int ExpiredPendingCount { get; set; }

        public decimal StockValueEstimate { get; set; }

        public List<LowStockItem> LowStockItems { get; private set; }

        public List<RecentSaleItem> RecentSales { get; private set; }
    }
}
