using Microsoft.Owin;
using System;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Slider
{
    public partial class SliderViewModel : BaseViewModel
    {
        public SliderViewModel() : base() { }

        public SliderViewModel(IOwinContext owinContext) : base (owinContext)
        {

        }


        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string Name { get; set; }

        public string Link { get; set; }

        [Display(Name = "Image", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public Nullable<int> MediaId { get; set; }

        [Display(Name = "OrderNumber", ResourceType = typeof(Commons))]
        public int OrderNumber { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public bool IsPublished { get; set; }

        public MediaViewModel GetImage()
        {
            MediaViewModel image = null;
            if (MediaId != null)
                image = _owinContext.GetMediaViewModelById(MediaId.Value);

            return image;
        }
    }
}