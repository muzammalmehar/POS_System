namespace ShopPOS.Models
{
    public class ProductSaveRequest
    {
        public int? ProductId { get; set; }

        public string ProductCode { get; set; }

        public string Barcode { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public int BaseUnitId { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public decimal ReorderLevel { get; set; }

        public bool TrackStock { get; set; }

        public bool TrackExpiry { get; set; }

        public int? DefaultShelfLifeDays { get; set; }

        public System.DateTime? DefaultExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public string ImagePath { get; set; }

        public int? PreferredVendorId { get; set; }
    }
}
