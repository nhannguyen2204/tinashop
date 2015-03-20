using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Brand
{
    public class BrandViewModel : BaseViewModel
    {
        [Display(Name = "BrandCode", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string BrandCode { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string Name { get; set; }
    }
}