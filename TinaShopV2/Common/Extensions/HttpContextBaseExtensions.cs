using Microsoft.AspNet.Identity;
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
                if (!string.IsNullOrEmpty(userId))
                    return ApplicationUserManager.Instance.FindById(userId);
            }
            return null;
        }
    }
}