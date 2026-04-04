namespace ShopPOS.Models
{
    public class SaleProductLookupItem
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string Barcode { get; set; }

        public string ProductName { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public decimal SalePrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal CurrentStock { get; set; }

        public bool TrackStock { get; set; }

        public bool TrackExpiry { get; set; }
    }
}
