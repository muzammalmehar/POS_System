using System;

namespace ShopPOS.Models
{
    public class VendorPurchaseHistoryItem
    {
        public string PurchaseNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string InvoiceNo { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal DueAmount { get; set; }

        public string Remarks { get; set; }
    }
}
