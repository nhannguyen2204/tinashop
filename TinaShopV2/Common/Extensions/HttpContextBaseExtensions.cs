using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using TinaShopV2.Models;

namespace TinaShopV2.Common.Extensions
{
    public static class HttpContextBaseExtensions
    {
        public static ApplicationUser GetCurrentUser(this HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                string userId = httpContext.User.Identity.GetUserId();

                var owinContext = httpContext.GetOwinContext();
                var userManager = owinContext.GetUserManager<ApplicationUserManager>();

                if (!string.IsNullOrEmpty(userId))
                    return userManager.FindById(userId);
            }
            return null;
        }
    }
}