using System;

namespace ShopPOS.Models
{
    public class VendorLedgerItem
    {
        public DateTime EntryDate { get; set; }

        public string EntryType { get; set; }

        public string ReferenceNo { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public string Remarks { get; set; }
    }
}
