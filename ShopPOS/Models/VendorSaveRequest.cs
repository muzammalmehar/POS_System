using System;

namespace ShopPOS.Models
{
    public class VendorSaveRequest
    {
        public int? SupplierId { get; set; }

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
    }
}
