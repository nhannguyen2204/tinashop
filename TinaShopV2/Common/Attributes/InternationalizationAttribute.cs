using System.Globalization;
using System.Threading;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using TinaShopV2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;

namespace TinaShopV2.Common.Attributes
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var owinContext = filterContext.HttpContext.GetOwinContext();
            var dbContext = owinContext.Get<ApplicationDbContext>();
            var userManager = owinContext.GetUserManager<ApplicationUserManager>();


            string language = (string)filterContext.RouteData.Values["language"] ?? "vi";
            string culture = (string)filterContext.RouteData.Values["culture"] ?? "VN";
            string fullCulture = string.Format("{0}-{1}", language, culture);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(fullCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(fullCulture);

            var routeData = filterContext.RequestContext.RouteData;

            string controllerName = routeData.Values["controller"] != null ? routeData.Values["controller"].ToString() : string.Empty;
            string actionName = routeData.Values["action"] != null ? routeData.Values["action"].ToString() : string.Empty;
            string areaName = routeData.Values["area"] != null ? routeData.Values["area"].ToString() : string.Empty;

            var tinaAction = dbContext.TinaActions.FirstOrDefault(m => m.Controller == controllerName && m.Action == actionName && m.Area == areaName);

            int tinaMenuId = 0;
            if (tinaAction != null)
            {
                var tinaMenu = dbContext.TinaMenus.FirstOrDefault(m => m.ActionId == tinaAction.Id);

                while (tinaMenu != null && tinaMenu.IsHidden)
                {
                    int? parentId = tinaMenu.ParentId;
                    tinaMenu = dbContext.TinaMenus.FirstOrDefault(m => m.Id == parentId);
                }

                if (tinaMenu != null)
                    tinaMenuId = tinaMenu.Id;
            }

            routeData.Values["TinaMenuId"] = tinaMenuId;
        }
    }
}