namespace ShopPOS.Models
{
    public class VendorProductLinkItem
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public decimal SalePrice { get; set; }

        public decimal LastPurchasePrice { get; set; }

        public bool IsLinked { get; set; }

        public bool IsPreferred { get; set; }
    }
}
