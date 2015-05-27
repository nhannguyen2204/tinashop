using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class Brand
    {
        public Brand()
        {
            this.Products = new List<Product>();
        }

        public string BrandCode { get; set; }
        public string Name { get; set; }
        public Nullable<int> MediaId { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public string SortName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
