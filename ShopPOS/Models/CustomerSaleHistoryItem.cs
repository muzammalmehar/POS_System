using System;

namespace ShopPOS.Models
{
    public class CustomerSaleHistoryItem
    {
        public string SaleNo { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal DueAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string Remarks { get; set; }
    }
}
