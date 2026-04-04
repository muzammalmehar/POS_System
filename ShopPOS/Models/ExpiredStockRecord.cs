using System;

namespace ShopPOS.Models
{
    public class ExpiredStockRecord
    {
        public long ExpiredRecordId { get; set; }

        public int ProductId { get; set; }

        public int? SupplierId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string SupplierName { get; set; }

        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public string ResolutionStatus { get; set; }

        public DateTime ProcessedAt { get; set; }

        public string Remarks { get; set; }
    }
}
