using System.Collections.Generic;
using System;

namespace ShopPOS.Models
{
    public class SaleSaveRequest
    {
        public SaleSaveRequest()
        {
            Items = new List<SaleCartItem>();
            SaleDate = DateTime.Now;
        }

        public int UserId { get; set; }

        public DateTime SaleDate { get; set; }

        public int? CustomerId { get; set; }

        public string PaymentMethod { get; set; }

        public int? WalletAccountId { get; set; }

        public string Remarks { get; set; }

        public decimal Discount { get; set; }

        public decimal ExtraCharges { get; set; }

        public decimal PaidAmount { get; set; }

        public List<SaleCartItem> Items { get; private set; }
    }
}
