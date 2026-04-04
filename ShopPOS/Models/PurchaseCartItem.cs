using System;

namespace ShopPOS.Models
{
    public class PurchaseCartItem
    {
        public int ProductId { get; set; }

        public long PurchaseDetailId { get; set; }

        public int UnitId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal SalePrice { get; set; }

        public bool TrackExpiry { get; set; }

        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public decimal LineTotal
        {
            get { return Quantity * Rate; }
        }
    }
}
