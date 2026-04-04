using System;

namespace ShopPOS.Models
{
    public class VendorPaymentReceipt
    {
        public long SupplierPaymentId { get; set; }

        public string ReceiptNo { get; set; }

        public string VendorName { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string WalletName { get; set; }

        public string Notes { get; set; }

        public string CreatedByName { get; set; }

        public decimal RemainingBalance { get; set; }
    }
}
