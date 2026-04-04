namespace ShopPOS.Models
{
    public class AccountBalanceItem
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountType { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal PeriodDebit { get; set; }

        public decimal PeriodCredit { get; set; }

        public decimal ClosingBalance { get; set; }

        public decimal DebitTotal { get; set; }

        public decimal CreditTotal { get; set; }

        public decimal Balance { get; set; }
    }
}
