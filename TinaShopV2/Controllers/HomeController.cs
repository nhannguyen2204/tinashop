using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Common;
using TinaShopV2.Models;

namespace TinaShopV2.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public HomeController()
            : base()
        {
            
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ComingSoon()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
