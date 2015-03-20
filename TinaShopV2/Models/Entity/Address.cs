using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class Address
    {
        public Address()
        {
            this.ProductDetails = new List<ProductDetail>();
        }

        public int Id { get; set; }
        public string StoreAddress { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
