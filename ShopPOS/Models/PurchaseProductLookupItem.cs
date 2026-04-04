namespace ShopPOS.Models
{
    public class PurchaseProductLookupItem
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string Barcode { get; set; }

        public string ProductName { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public bool TrackExpiry { get; set; }

        public int? DefaultShelfLifeDays { get; set; }

        public System.DateTime? DefaultExpiryDate { get; set; }

        public int? PreferredVendorId { get; set; }

        public string PreferredVendorName { get; set; }
    }
}
