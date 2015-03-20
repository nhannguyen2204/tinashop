using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class BillDetail
    {
        public int BillId { get; set; }
        public string ProductCode { get; set; }
        public int StoreAddressId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> CashDiscount { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
