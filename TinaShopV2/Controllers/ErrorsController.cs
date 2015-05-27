using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TinaShopV2.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult HttpError()
        {
            return View();
        }

        public ActionResult Http500()
        {
            return View();
        }

        public ActionResult Http404()
        {
            return View();
        }
    }
}