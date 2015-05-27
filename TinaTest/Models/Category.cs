using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public string CatCode { get; set; }
        public string CatParentCode { get; set; }
        public int OrderNumber { get; set; }
        public string Name { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
