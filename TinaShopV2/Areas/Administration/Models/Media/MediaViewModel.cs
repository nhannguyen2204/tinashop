using Foolproof;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Media
{
    public class MediaViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string Name { get; set; }

        [Display(Name = "File")]
        [DataType(DataType.ImageUrl)]
        public string FilePath { get; set; }

        [Display(Name = "File")]
        [RequiredIfEmpty("FilePath", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Common.Attributes.Validation.FileExtensions("jpg|gif|jpeg|png|bmp", ErrorMessageResourceName = "ImageExtension", ErrorMessageResourceType = typeof(Errors))]
        public HttpPostedFileBase FileUploader { get; set; }

        [Display(Name = "Thumbnail")]
        [DataType(DataType.ImageUrl)]
        public string ThumbPath { get; set; }

        [Display(Name = "Thumbnail")]
        [RequiredIfEmpty("ThumbPath", ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Common.Attributes.Validation.FileExtensions("jpg|gif|jpeg|png|bmp", ErrorMessageResourceName = "ImageExtension", ErrorMessageResourceType = typeof(Errors))]
        public HttpPostedFileBase ThumbUploader { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "MediaType", ResourceType = typeof(Commons))]
        public int TypeId { get; set; }

        public TinaShopV2.Models.Entity.MediaType MediaTypeObj
        {
            get 
            {
                return ApplicationDbContext.Instance.MediaTypes.Find(TypeId); 
            }
        }

        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Product", ResourceType = typeof(Commons))]
        public string ProductCode { get; set; }

        public TinaShopV2.Models.Entity.Product ProductObj
        {
            get
            {
                if (!string.IsNullOrEmpty(ProductCode))
                    return ApplicationDbContext.Instance.Products.Find(this.ProductCode);

                return null;
            }
        }
    }
}