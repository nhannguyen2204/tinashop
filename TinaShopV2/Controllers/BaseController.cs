using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;

namespace TinaShopV2.Controllers
{
    public class BaseController : Controller
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return ApplicationUserManager.Instance;
            }
        }

        private ApplicationUser currentUser;
        public ApplicationUser CurrentUser
        {
            get 
            {
                if (currentUser == null)
                    currentUser = HttpContext.GetCurrentUser();

                return currentUser; 
            }
            private set { currentUser = value; }
        }

    }
}