using System;
using System.Collections.Generic;

namespace TinaTest.Models
{
    public partial class Product
    {
        public Product()
        {
            this.Medias = new List<Media>();
            this.ProductDetails = new List<ProductDetail>();
        }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string BrandCode { get; set; }
        public bool CanSale { get; set; }
        public decimal Price { get; set; }
        public Nullable<bool> IsPublish { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public string UpdatedUserId { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public System.DateTime UpdatedDatetime { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Media> Medias { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
