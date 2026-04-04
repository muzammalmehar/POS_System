using System;

namespace ShopPOS.Models
{
    public class VendorPaymentRequest
    {
        public int SupplierId { get; set; }

        public int WalletAccountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Notes { get; set; }

        public DateTime? NextPaymentDate { get; set; }

        public int UserId { get; set; }
    }
}
