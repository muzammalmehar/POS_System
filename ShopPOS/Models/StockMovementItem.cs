using System;

namespace ShopPOS.Models
{
    public class StockMovementItem
    {
        public DateTime CreatedAt { get; set; }

        public string TransactionType { get; set; }

        public decimal QtyIn { get; set; }

        public decimal QtyOut { get; set; }

        public decimal UnitCost { get; set; }

        public string Remarks { get; set; }

        public string CreatedByName { get; set; }
    }
}
