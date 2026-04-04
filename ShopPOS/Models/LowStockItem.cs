namespace ShopPOS.Models
{
    public class LowStockItem
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public decimal CurrentStock { get; set; }

        public decimal ReorderLevel { get; set; }

        public string Status
        {
            get
            {
                if (CurrentStock <= 0)
                {
                    return "Out of stock";
                }

                return "Low stock";
            }
        }
    }
}
