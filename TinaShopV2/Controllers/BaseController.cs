using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Common;

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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsLocal && !Request.Url.Host.ToLower().Equals(GlobalObjects.MainDomain))
                Response.Redirect(string.Format("{0}{1}", GlobalObjects.MainDomainProtocol, Request.Url.PathAndQuery));

            if (GlobalObjects.IsComingSoonMode && !Request.IsLocal && !Request.Url.PathAndQuery.ToLower().Equals(Url.Action("ComingSoon", "Home", new { area = "" }).ToLower()))
                Response.Redirect(Url.Action("ComingSoon", "Home", new { area = "" }));

            base.OnActionExecuting(filterContext);
        }
    }
}