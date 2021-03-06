﻿using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using TinaShopV2.Common;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Models;

namespace TinaShopV2.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationUserManager _userManagerService;
        protected ApplicationDbContext _dbContextService;
        protected IOwinContext _owinContext;

        public BaseController()
        {
            
        }

        private ApplicationUser currentUser;
        public ApplicationUser CurrentUser
        {
            get
            {
                return currentUser;
            }
            private set { currentUser = value; }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            _owinContext = requestContext.HttpContext.GetOwinContext();
            _userManagerService = _owinContext.GetUserManager<ApplicationUserManager>();
            _dbContextService = _owinContext.Get<ApplicationDbContext>();

            if (currentUser == null)
                currentUser = HttpContext.GetCurrentUser();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsLocal && !Request.Url.Host.ToLower().Equals(GlobalObjects.MainDomain))
                Response.Redirect(string.Format("{0}{1}", GlobalObjects.MainDomainProtocol, Request.Url.PathAndQuery));

            var routeData = filterContext.RequestContext.RouteData;
            string areaName = routeData.Values["area"] != null ? routeData.Values["area"].ToString() : string.Empty;

            if (GlobalObjects.IsComingSoonMode && !Request.IsLocal && !areaName.ToLower().Equals("administration") && !filterContext.Controller.ControllerContext.IsChildAction &&
                !Request.Url.PathAndQuery.ToLower().Equals(Url.Action("ComingSoon", "Home", new { area = "" }).ToLower()))
                Response.Redirect(Url.Action("ComingSoon", "Home", new { area = "" }));

            base.OnActionExecuting(filterContext);
        }
    }
}