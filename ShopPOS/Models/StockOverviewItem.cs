namespace ShopPOS.Models
{
    public class StockOverviewItem
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SalePrice { get; set; }

        public decimal ReorderLevel { get; set; }

        public decimal CurrentStock { get; set; }

        public bool TrackExpiry { get; set; }

        public decimal StockValue
        {
            get { return CurrentStock * PurchasePrice; }
        }

        public string StockStatus
        {
            get
            {
                if (CurrentStock <= 0)
                {
                    return "Out of stock";
                }

                if (CurrentStock <= ReorderLevel)
                {
                    return "Low stock";
                }

                return "Normal";
            }
        }
    }
}
