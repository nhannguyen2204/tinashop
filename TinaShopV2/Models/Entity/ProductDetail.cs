using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class ProductDetail
    {
        public string ProductCode { get; set; }
        public int AddressId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public virtual Address Address { get; set; }
        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
    }
}
