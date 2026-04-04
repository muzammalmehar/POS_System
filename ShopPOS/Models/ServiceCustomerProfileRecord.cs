using System;

namespace ShopPOS.Models
{
    public class ServiceCustomerProfileRecord
    {
        public int ServiceCustomerProfileId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerMobile { get; set; }

        public string ReferenceNumber { get; set; }

        public string BillCategory { get; set; }

        public int? ServiceTypeId { get; set; }

        public string ServiceTypeName { get; set; }

        public int? WalletAccountId { get; set; }

        public string WalletName { get; set; }

        public decimal DefaultAmount { get; set; }

        public decimal DefaultServiceCharge { get; set; }

        public string RecurrenceType { get; set; }

        public int? ExpectedDayOfMonth { get; set; }

        public DateTime? NextDueDate { get; set; }

        public DateTime? LastServiceDate { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string DueStatus
        {
            get
            {
                if (!NextDueDate.HasValue)
                {
                    return "No Reminder";
                }

                if (NextDueDate.Value.Date < DateTime.Today)
                {
                    return "Overdue";
                }

                if (NextDueDate.Value.Date == DateTime.Today)
                {
                    return "Due Today";
                }

                return "Upcoming";
            }
        }
    }
}
