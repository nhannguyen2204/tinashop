using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models;
using TinaShopV2.Areas.Administration.Models.User;
using TinaShopV2.Common;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    public class ErrorsController : Controller
    {
        [AllowAnonymous]
        public ActionResult HttpError()
        {
            Response.StatusCode = 500;
            return View("Http500");
        }

        [AllowAnonymous]
        public ActionResult Http401()
        {
            var routeData = HttpContext.Request.RequestContext.RouteData;
            TempData[GlobalObjects.ErrorMessageKey] = App_GlobalResources.Errors.Http401;
            return RedirectToAction("LogIn", "Account", new { @area = "Administration", @ReturnUrl = HttpContext.Request.RawUrl });
        }

        [AllowAnonymous]
        public ActionResult Http404()
        {
            Response.StatusCode = 404;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Http500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}