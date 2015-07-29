using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TinaShopV2.Common;
using TinaShopV2.Models;
using TinaShopV2.Models.Entity;

namespace TinaShopV2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "HomeContact",
                url: "lien-he",
                defaults: new
                {
                    area = "",
                    controller = "Home",
                    action = "Contact",
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

            routes.MapRoute(
                name: "HomeAbout",
                url: "gioi-thieu",
                defaults: new
                {
                    area = "",
                    controller = "Home",
                    action = "About",
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

            routes.MapRoute(
                name: "HomeProductsDetails",
                url: "san-pham/chi-tiet/{ProductCode}",
                defaults: new
                {
                    area = "",
                    controller = "Products",
                    action = "Details",
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

            routes.MapRoute(
                name: "HomeProducts",
                url: "san-pham/{Page}/{BrandCode}/{CatCode}/{ColorKeys}/{FromPrice}/{ToPrice}/{IsOrderDatetimeDesc}/{IsOrderPriceDesc}",
                defaults: new
                {
                    area = "",
                    controller = "Products",
                    action = "Index",
                    Page = 1,
                    FromPrice = 0,
                    ToPrice = 3000,
                    BrandCode = GlobalObjects.DefaultAllBrandCode,
                    CatCode = GlobalObjects.DefaultAllCatCode,
                    ColorKeys = GlobalObjects.DefaultAllColors,
                    IsOrderDatetimeDesc = 0,
                    IsOrderPriceDesc = 0,
                    language = "vi",
                    culture = "VN"
                }, namespaces: new[] { "TinaShopV2.Controllers" }
            );

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

            GenerateMapping();
        }

        private static void GenerateMapping()
        {
            AutoMapper.Mapper.CreateMap<Product, ProductFOViewModel>();
            AutoMapper.Mapper.CreateMap<ProductFOViewModel, Product>();
        }
    }
}
