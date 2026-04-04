using System;

namespace ShopPOS.Models
{
    public class UnifiedRecentSaleItem
    {
        public string SaleType { get; set; }

        public long RecordId { get; set; }

        public string DocumentNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CustomerName { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal ProfitAmount { get; set; }

        public string PaymentInfo { get; set; }

        public string Status { get; set; }

        public string CashierName { get; set; }

        public string Remarks { get; set; }
    }
}
