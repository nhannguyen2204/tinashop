using System;
using System.Collections.Generic;

namespace TinaShopV2.Models.Entity
{
    public partial class Catalog
    {
        public Catalog()
        {
            this.Products = new List<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> MediaId { get; set; }
        public bool IsPublished { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}