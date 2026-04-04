using System;

namespace ShopPOS.Models
{
    public class ExpenseRecord
    {
        public long ExpenseId { get; set; }

        public string ExpenseTypeName { get; set; }

        public DateTime ExpenseDate { get; set; }

        public decimal Amount { get; set; }

        public string WalletName { get; set; }

        public string Description { get; set; }

        public string CreatedByName { get; set; }
    }
}
