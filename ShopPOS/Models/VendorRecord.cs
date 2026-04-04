using System;

namespace ShopPOS.Models
{
    public class VendorRecord
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal OpeningBalance { get; set; }

        public string BalanceType { get; set; }

        public bool IsActive { get; set; }

        public string PreferredVisitDay { get; set; }

        public string PaymentCycle { get; set; }

        public int CreditDays { get; set; }

        public DateTime? NextPaymentDate { get; set; }

        public string Notes { get; set; }

        public decimal PurchaseAmount { get; set; }

        public decimal PurchaseDueAmount { get; set; }

        public decimal PaymentPaidAmount { get; set; }

        public decimal NetBalance { get; set; }

        public int ExpiryPendingCount { get; set; }

        public int ExpiryReturnedCount { get; set; }

        public int ExpiryBurntCount { get; set; }

        public string BalanceStatus
        {
            get
            {
                if (NetBalance > 0)
                {
                    return "Payable";
                }

                if (NetBalance < 0)
                {
                    return "Receivable";
                }

                return "Settled";
            }
        }
    }
}
