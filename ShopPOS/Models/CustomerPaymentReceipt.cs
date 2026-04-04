using System;

namespace ShopPOS.Models
{
    public class CustomerPaymentReceipt
    {
        public long CustomerPaymentId { get; set; }

        public string ReceiptNo { get; set; }

        public string CustomerName { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string WalletName { get; set; }

        public string Remarks { get; set; }

        public string CreatedByName { get; set; }

        public decimal RemainingReceivable { get; set; }
    }
}
