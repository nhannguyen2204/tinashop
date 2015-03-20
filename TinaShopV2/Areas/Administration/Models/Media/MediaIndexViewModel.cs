using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Media
{
    public class MediaIndexViewModel : IndexBasicViewModel
    {
        [Display(Name = "MediaType", ResourceType = typeof(Commons))]
        public int? TypeId { get; set; }

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

        public IEnumerable<MediaViewModel> Medias { get; set; }
    }
}