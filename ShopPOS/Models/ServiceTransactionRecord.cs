using System;

namespace ShopPOS.Models
{
    public class ServiceTransactionRecord
    {
        public long ServiceTransactionId { get; set; }

        public string TransactionNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public string ServiceName { get; set; }

        public string ProviderName { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string ReferenceNumber { get; set; }

        public string BillCategory { get; set; }

        public string WalletName { get; set; }

        public string PaymentMethod { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string ExternalTransactionId { get; set; }

        public decimal Amount { get; set; }

        public decimal ServiceCharge { get; set; }

        public decimal CommissionEarned { get; set; }

        public decimal NetEffectAmount { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public string CreatedByName { get; set; }
    }
}
