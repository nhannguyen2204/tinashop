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
            this.Catalogs = new List<Catalog>();
        }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string CatCode { get; set; }
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
        public virtual Category Category { get; set; }
        public virtual ICollection<Media> Medias { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<Catalog> Catalogs { get; set; }
    }
}
