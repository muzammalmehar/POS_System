using System;

namespace ShopPOS.Models
{
    public class VendorDueItem
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string VisitDay { get; set; }

        public string PaymentCycle { get; set; }

        public DateTime? NextPaymentDate { get; set; }

        public decimal OutstandingAmount { get; set; }
    }
}
