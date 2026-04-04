using System;

namespace ShopPOS.Models
{
    public class LedgerVoucherItem
    {
        public DateTime TransactionDate { get; set; }

        public string VoucherType { get; set; }

        public string ReferenceTable { get; set; }

        public long? ReferenceId { get; set; }

        public string ReferenceLabel { get; set; }

        public decimal TotalAmount { get; set; }

        public string Remarks { get; set; }
    }
}
