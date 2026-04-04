namespace ShopPOS.Models
{
    public class SaleCartItem
    {
        public int ProductId { get; set; }

        public int UnitId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public decimal AvailableStock { get; set; }

        public bool TrackStock { get; set; }

        public bool TrackExpiry { get; set; }

        public decimal CostRate { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal LineTotal
        {
            get { return Quantity * Rate; }
        }
    }
}
