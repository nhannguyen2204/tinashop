using System.ComponentModel.DataAnnotations;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.User
{
    public class CreateUserViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Username", ResourceType = typeof(Commons))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Commons))]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string[] RoleId { get; set; }
    }
}