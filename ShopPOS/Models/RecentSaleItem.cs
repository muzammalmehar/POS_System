using System;

namespace ShopPOS.Models
{
    public class RecentSaleItem
    {
        public string SaleNo { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal GrandTotal { get; set; }

        public string PaymentMethod { get; set; }

        public string Cashier { get; set; }
    }
}
