using System;
using System.Collections.Generic;

namespace ShopPOS.Models
{
    public class SaleEditRecord
    {
        public SaleEditRecord()
        {
            Items = new List<SaleCartItem>();
        }

        public long SaleId { get; set; }

        public string SaleNo { get; set; }

        public DateTime SaleDate { get; set; }

        public int? CustomerId { get; set; }

        public string PaymentMethod { get; set; }

        public int? WalletAccountId { get; set; }

        public string Remarks { get; set; }

        public decimal Discount { get; set; }

        public decimal ExtraCharges { get; set; }

        public decimal PaidAmount { get; set; }

        public bool IsRefunded { get; set; }

        public List<SaleCartItem> Items { get; private set; }
    }
}
