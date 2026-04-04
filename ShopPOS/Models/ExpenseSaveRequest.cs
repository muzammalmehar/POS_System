using System;

namespace ShopPOS.Models
{
    public class ExpenseSaveRequest
    {
        public int ExpenseTypeId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public decimal Amount { get; set; }

        public int WalletAccountId { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
