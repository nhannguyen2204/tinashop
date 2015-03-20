using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.MediaType
{
    public class MediaTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Commons))]
        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public string Name { get; set; }
    }
}