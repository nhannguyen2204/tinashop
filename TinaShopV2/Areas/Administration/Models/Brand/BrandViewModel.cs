using System;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using Microsoft.Owin;

namespace TinaShopV2.Areas.Administration.Models.Brand
{
    public class BrandViewModel : BaseViewModel
    {
        public BrandViewModel() : base() { }

        public BrandViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            
        }

        [Display(Name = "BrandCode", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string BrandCode { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string Name { get; set; }

        [Display(Name = "Image", ResourceType = typeof(Commons))]
        public Nullable<int> MediaId { get; set; }

        public MediaViewModel GetImage()
        {
            MediaViewModel image = null;
            if (MediaId != null)
                image = _owinContext.GetMediaViewModelById(MediaId.Value);
            
            return image;
        }
    }
}