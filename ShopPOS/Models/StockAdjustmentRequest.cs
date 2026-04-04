namespace ShopPOS.Models
{
    public class StockAdjustmentRequest
    {
        public int ProductId { get; set; }

        public string TransactionType { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public string Remarks { get; set; }

        public int UserId { get; set; }
    }
}
