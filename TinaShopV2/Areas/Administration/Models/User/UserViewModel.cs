using Microsoft.Owin;
using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.User
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel() : base() { }

        public UserViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            
        }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Username", ResourceType = typeof(Commons))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Commons))]
        public string PhoneNumber { get; set; }

        [Display(Name = "UserLockoutEnabled", ResourceType = typeof(App_GlobalResources.Commons))]
        public bool LockoutEnabled { get; set; }
    }
}