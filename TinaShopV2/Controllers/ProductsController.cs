using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TinaShopV2.Common;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Models;

namespace TinaShopV2.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController() : base() { }

        // GET: Products
        public ActionResult Index(ProductFilterIndexViewModel model)
        {
            _owinContext.GetProductFOViewModels(ref model);

            if (Request.IsAjaxRequest())
            {
                // ajax request handled
                return PartialView("_ProductListingPartial", model);
            }
            return View(model);
        }

        public ActionResult Details(string productCode)
        {
            var model = _owinContext.GetProductFOViewModelById(productCode);
            return View(model);
        }

        public ActionResult ProductListing(ProductFilterIndexViewModel model)
        {
            return PartialView("_ProductListingPartial", model);
        }

        public ActionResult BrandListing(ProductFilterIndexViewModel model)
        {
            return PartialView("_BrandListingPartial", model);
        }

        public ActionResult CategoryListing(ProductFilterIndexViewModel model)
        {
            return PartialView("_CategoryListingPartial", model);
        }

        public ActionResult ColorListing(ProductFilterIndexViewModel model)
        {
            return PartialView("_ColorListingPartial", model);
        }

        public ActionResult PriceFilter(ProductFilterIndexViewModel model)
        {
            return PartialView("_PriceFilterPartial", model);
        }

        public JsonResult GetSortUrlByModel(ProductFilterIndexViewModel model)
        {
            model.Products = null;
            string url = Url.GetUrlByModel(model);

            return new JsonResult() { Data = new BasicJsonResponse() { Result = url }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}