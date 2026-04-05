using System;

namespace ShopPOS.Models
{
    public class ServiceTransactionSaveRequest
    {
        public long? ServiceTransactionId { get; set; }

        public int ServiceTypeId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string ReferenceNumber { get; set; }

        public string BillCategory { get; set; }

        public int WalletAccountId { get; set; }

        public string PaymentMethod { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string ExternalTransactionId { get; set; }

        public bool IsExternalTransactionIdNotApplicable { get; set; }

        public decimal Amount { get; set; }

        public decimal ServiceCharge { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        public int UserId { get; set; }

        public bool IsWalkInCustomer { get; set; }

        public int? ProfileId { get; set; }

        public bool SaveProfile { get; set; }

        public string RecurrenceType { get; set; }

        public int? ExpectedDayOfMonth { get; set; }

        public DateTime? NextDueDate { get; set; }
    }
}
