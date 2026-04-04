using System;

namespace ShopPOS.Models
{
    public class VendorExpiredReturnItem
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal Quantity { get; set; }

        public string ResolutionStatus { get; set; }

        public DateTime ProcessedAt { get; set; }

        public string Remarks { get; set; }
    }
}
