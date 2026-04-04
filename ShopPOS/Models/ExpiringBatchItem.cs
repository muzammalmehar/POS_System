using System;

namespace ShopPOS.Models
{
    public class ExpiringBatchItem
    {
        public long BatchId { get; set; }

        public int ProductId { get; set; }

        public int? SupplierId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string SupplierName { get; set; }

        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal RemainingQty { get; set; }

        public decimal UnitCost { get; set; }

        public string AgeStatus { get; set; }
    }
}
