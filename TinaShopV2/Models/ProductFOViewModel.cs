using Microsoft.Owin;
using System;
using System.Collections.Generic;
using TinaShopV2.Models.Entity;
using System.Linq;
using TinaShopV2.Common;

namespace TinaShopV2.Models
{
    public class ProductFOViewModel : BaseViewModel
    {
        public ProductFOViewModel()
            : base()
        {
            Init();
        }

        public ProductFOViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            Init();
        }

        private void Init()
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

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Media> Medias { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<Catalog> Catalogs { get; set; }

        public IEnumerable<TinaShopV2.Models.Entity.Media> Get_Images()
        {
            if (!string.IsNullOrEmpty(ProductCode))
            {
                var images = _dbContextService.Medias.Where(m => m.TypeId == GlobalObjects.MediaType_ProductImage_Id && m.ProductCode == ProductCode).OrderBy(m => m.Name);
                if (images.Count() > 0)
                    return images;
            }
            return new List<TinaShopV2.Models.Entity.Media>() { };
        }
    }
}