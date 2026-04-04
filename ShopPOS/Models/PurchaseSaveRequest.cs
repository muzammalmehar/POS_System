using System;
using System.Collections.Generic;

namespace ShopPOS.Models
{
    public class PurchaseSaveRequest
    {
        public PurchaseSaveRequest()
        {
            Items = new List<PurchaseCartItem>();
        }

        public int SupplierId { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal Discount { get; set; }

        public decimal OtherCharges { get; set; }

        public decimal PaidAmount { get; set; }

        public int? WalletAccountId { get; set; }

        public string Remarks { get; set; }

        public int UserId { get; set; }

        public List<PurchaseCartItem> Items { get; private set; }
    }
}
