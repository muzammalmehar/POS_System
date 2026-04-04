namespace ShopPOS.Models
{
    public class CustomerRecord
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string ImagePath { get; set; }

        public decimal OpeningBalance { get; set; }

        public string BalanceType { get; set; }

        public bool IsActive { get; set; }

        public decimal PurchaseAmount { get; set; }

        public decimal SaleDueAmount { get; set; }

        public decimal PaymentReceivedAmount { get; set; }

        public decimal NetBalance { get; set; }

        public string BalanceStatus
        {
            get
            {
                if (NetBalance > 0)
                {
                    return "Receivable";
                }

                if (NetBalance < 0)
                {
                    return "Payable";
                }

                return "Clear";
            }
        }
    }
}
