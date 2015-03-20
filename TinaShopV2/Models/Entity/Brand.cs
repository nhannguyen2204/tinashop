using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class Brand
    {
        public Brand()
        {
            this.Products = new List<Product>();
        }

        public string BrandCode { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public string SortName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
