using Microsoft.Owin;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.MenuType
{
    public class MenuTypeViewModel : BaseViewModel
    {
        public MenuTypeViewModel() : base() { }

        public MenuTypeViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Name", ResourceType = typeof(App_GlobalResources.Commons))]
        public string Name { get; set; }
    }
}