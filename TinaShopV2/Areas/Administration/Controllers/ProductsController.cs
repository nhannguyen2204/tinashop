using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.Product;
using TinaShopV2.Controllers;
using TinaShopV2.Models;
using TinaShopV2.Models.Entity;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models.CustomControls;
using TinaShopV2.Areas.Administration.Models.Category;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class ProductsController : BaseController
    {

        public ProductsController()
            : base()
        {

        }

        public JsonResult Find(string productCode)
        {
            IEnumerable<ProductViewModel> result = null;

            if (!string.IsNullOrEmpty(productCode))
                result = _owinContext.FindProductViewModelById(productCode);

            List<ResponseProductViewModel> model = new List<ResponseProductViewModel>();
            AutoMapper.Mapper.Map(result, model);
            ;
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Administration/Products
        public ActionResult Index(ProductIndexViewModel model)
        {
            // Init Products
            _owinContext.GetProductsByIndexViewModel(ref model);

            // Init Lists for View
            ViewBag.PublishStatusList = EnumHelper<PublishStatus>.EnumToList();
            ViewBag.DeleteStatusList = EnumHelper<DeleteStatus>.EnumToList();
            ViewBag.SaleStateList = EnumHelper<SaleState>.EnumToList();
            ViewBag.Brands = _owinContext.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
            
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            // Response
            return View(model);
        }

        [ActionName("Index"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult SearchProduct(ProductIndexViewModel model)
        {
            model.Page = 1;
            return Redirect(Url.Action("Index", "Products", model));
        }

        // GET: Administration/Products/Details/5
        public ActionResult Details(string productCode)
        {
            var model = _owinContext.GetProductViewModelById(productCode);
            return View(model);
        }

        // GET: Administration/Products/Create
        public ActionResult Create()
        {
            ViewBag.Brands = _owinContext.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode, Text = m.Name });
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            return View();
        }

        // POST: Administration/Products/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateProductByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Brands = _owinContext.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode, Text = m.Name });
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            return View(model);
        }

        // GET: Administration/Products/Edit/5
        public ActionResult Edit(string productCode)
        {
            var model = _owinContext.GetProductViewModelById(productCode);

            ViewBag.Brands = _owinContext.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode, Text = m.Name });
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            return View(model);
        }

        // POST: Administration/Products/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SetOwinContext(_owinContext);
                    
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditProductByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Brands = _owinContext.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode, Text = m.Name });
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });
            return View(model);
        }

        // GET: Administration/Products/Delete/5
        public ActionResult Delete(string productCode)
        {
            var model = _owinContext.GetProductViewModelById(productCode);
            return View(model);
        }

        // POST: Administration/Products/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string productCode)
        {
            var model = _owinContext.GetProductViewModelById(productCode);

            try
            {
                _owinContext.DeleteProductById(productCode);
                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }
    }
}
