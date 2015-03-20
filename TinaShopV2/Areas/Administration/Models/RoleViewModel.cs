using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;

namespace TinaShopV2.Areas.Administration.Models
{
    public class RoleViewModel
    {
        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Name", ResourceType = typeof(Commons))]
        public string Name { get; set; }
    }
}