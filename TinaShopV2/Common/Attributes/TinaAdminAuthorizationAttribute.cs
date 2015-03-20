using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using System.Threading.Tasks;

namespace TinaShopV2.Common.Attributes
{
    public class TinaAdminAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            var currentUser = httpContext.GetCurrentUser();
            var routeData = httpContext.Request.RequestContext.RouteData;

            string controllerName = routeData.Values["controller"] != null ? routeData.Values["controller"].ToString() : string.Empty;
            string actionName = routeData.Values["action"] != null ? routeData.Values["action"].ToString() : string.Empty;
            string areaName = routeData.Values["area"] != null ? routeData.Values["area"].ToString() : string.Empty;

            var tinaAction = ApplicationDbContext.Instance.TinaActions.FirstOrDefault(m => m.Controller == controllerName && m.Action == actionName && m.Area == areaName);
            if (tinaAction != null)
            {
                if (tinaAction.IsAnonymous)
                    return true;

                var roleIds = ApplicationDbContext.Instance.TinaAuthorizes.Where(m => m.ActionId == tinaAction.Id).Select(m => m.RoleId).ToArray();
                var roleNames = ApplicationDbContext.Instance.Roles.Where(m => roleIds.Contains(m.Id)).Select(m => m.Name).ToArray();
                if (currentUser != null && roleNames.Count() > 0)
                {
                    foreach (var roleName in roleNames)
                    {
                        var task = ApplicationUserManager.Instance.IsInRoleAsync(currentUser.Id,roleName);
                        if (task.Result)
                        {
                            return true;
                        }
                    }
                }
                else if (currentUser != null)
                {
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(AdminGlobalObjects.HostUser) && currentUser != null && currentUser.UserName.ToLower() == AdminGlobalObjects.HostUser.ToLower())
                return true;

            return false;
        }
    }
}