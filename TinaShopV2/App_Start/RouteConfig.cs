using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TinaShopV2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ComingSoon",
                url: "ComingSoon",
                defaults: new
                {
                    area = "",
                    controller = "Home",
                    action = "ComingSoon",
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    area = "",
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

            routes.MapRoute(
                name: "Default_Localized",
                url: "{language}-{culture}/{controller}/{action}/{id}",
                defaults: new
                    {
                        area = "",
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional,
                        language = "vi",
                        culture = "VN"
                    }, namespaces: new[] { "TinaShopV2.Controllers" }
            );
        }
    }
}
