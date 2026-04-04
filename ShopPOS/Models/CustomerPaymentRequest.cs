using System;

namespace ShopPOS.Models
{
    public class CustomerPaymentRequest
    {
        public int CustomerId { get; set; }

        public int WalletAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Remarks { get; set; }

        public int UserId { get; set; }
    }
}
