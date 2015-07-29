using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Common;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Product
{
    public class ProductViewModel : BaseViewModel
    {
        public ProductViewModel() : base() { }

        public ProductViewModel(IOwinContext owinContext)
            : base(owinContext)
        {

        }

        private string imagePath;
        [Display(Name = "Image", ResourceType = typeof(Commons))]
        public string ImagePath
        {
            get
            {
                imagePath = Path.Combine(GlobalObjects.MediaImageFolderPath, GlobalObjects.Get_Media_NoImage(_owinContext).FilePath);

                TinaShopV2.Models.Entity.Media image = null;
                if (image == null && !string.IsNullOrEmpty(this.ProductCode))
                    image = _dbContextService.Medias.Where(m => m.ProductCode == this.ProductCode && m.TypeId == GlobalObjects.MediaType_ProductImage_Id).OrderBy(m => m.Name).FirstOrDefault();

                if (image != null)
                    imagePath = Path.Combine(GlobalObjects.MediaImageFolderPath, image.FilePath);

                return imagePath;
            }
        }

        private string imageThumbPath;
        [Display(Name = "Image", ResourceType = typeof(Commons))]
        public string ImageThumbPath
        {
            get
            {
                imageThumbPath = Path.Combine(GlobalObjects.MediaImageFolderPath, GlobalObjects.Get_Media_NoImage(_owinContext).ThumbPath);

                TinaShopV2.Models.Entity.Media image = null;
                if (image == null && !string.IsNullOrEmpty(this.ProductCode))
                    image = _dbContextService.Medias.Where(m => m.ProductCode == this.ProductCode && m.TypeId == GlobalObjects.MediaType_ProductImage_Id).OrderBy(m => m.Name).FirstOrDefault();

                if (image != null)
                    imageThumbPath = Path.Combine(GlobalObjects.MediaImageFolderPath, image.ThumbPath);

                return imageThumbPath;
            }
        }

        public IEnumerable<TinaShopV2.Models.Entity.Media> Get_Images()
        {
            if (!string.IsNullOrEmpty(ProductCode))
            {
                var images = _dbContextService.Medias.Where(m => m.TypeId == GlobalObjects.MediaType_ProductImage_Id && m.ProductCode == ProductCode).OrderBy(m => m.Name);
                if (images.Count() > 0)
                    return images;
            }
            return new List<TinaShopV2.Models.Entity.Media>() {  };
        }

        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "ProductCode", ResourceType = typeof(Commons))]
        public string ProductCode { get; set; }

        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "ProductName", ResourceType = typeof(Commons))]
        public string ProductName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Commons))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Brand", ResourceType = typeof(Commons))]
        public string BrandCode { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Category", ResourceType = typeof(Commons))]
        public string CatCode { get; set; }

        public TinaShopV2.Models.Entity.Brand Get_Brand()
        {
            return _dbContextService.Brands.Find(BrandCode);
        }

        public TinaShopV2.Models.Entity.Category Get_Category()
        {
            return _dbContextService.Categories.Find(CatCode);
        }

        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        public bool CanSale { get; set; }

        [Range(typeof(decimal), "0", "9999999999", ErrorMessageResourceName = "ValueBetween", ErrorMessageResourceType = typeof(Errors))]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "InvalidCurrency", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Price", ResourceType = typeof(Commons))]
        [DisplayFormat(DataFormatString = "{0:#}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        public bool IsPublish { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Commons))]
        public bool IsDeleted { get; set; }
    }

    public class ResponseProductViewModel
    {
        [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "ProductCode", ResourceType = typeof(Commons))]
        public string ProductCode { get; set; }

        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "ProductName", ResourceType = typeof(Commons))]
        public string ProductName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Commons))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Brand", ResourceType = typeof(Commons))]
        public string BrandCode { get; set; }

        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        public bool CanSale { get; set; }

        [Range(typeof(decimal), "0", "9999999999", ErrorMessageResourceName = "ValueBetween", ErrorMessageResourceType = typeof(Errors))]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "InvalidCurrency", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Price", ResourceType = typeof(Commons))]
        [DisplayFormat(DataFormatString = "{0:#}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        public bool IsPublish { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Commons))]
        public bool IsDeleted { get; set; }
    }
}