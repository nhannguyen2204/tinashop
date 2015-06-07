using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.Category;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class CategoryController : BaseController
    {
        public CategoryController()
            : base()
        {

        }

        // GET: Administration/Category
        public ActionResult Index()
        {
            List<CategoryViewModel> model = new List<CategoryViewModel>();

            // Generate Categories
            AdminHelpers.GenerateCategories(ref model, _owinContext);

            return View(model);
        }

        // GET: Administration/Category/Details/5
        public ActionResult Details(string catCode)
        {
            CategoryViewModel model = _owinContext.GetCategoryViewModelByCatCode(catCode);
            if (model == null)
                throw new HttpException(404, "ContentNotFound");

            return View(model);
        }

        // GET: Administration/Category/Create
        public ActionResult Create()
        {
            List<CategoryViewModel> categories = new List<CategoryViewModel>();

            // Generate Categories
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            return View(new CategoryViewModel(_owinContext));
        }

        // POST: Administration/Category/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateCategoryByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }

                List<CategoryViewModel> categories = new List<CategoryViewModel>();
                // Generate Categories
                AdminHelpers.GenerateCategories(ref categories, _owinContext);
                ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

                return View(model);
            }
            catch (Exception ex)
            {
                List<CategoryViewModel> categories = new List<CategoryViewModel>();
                // Generate Categories
                AdminHelpers.GenerateCategories(ref categories, _owinContext);
                ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Category/Edit/5
        public ActionResult Edit(string catCode)
        {
            CategoryViewModel model = _owinContext.GetCategoryViewModelByCatCode(catCode);
            if (model == null)
                throw new HttpException(404, "ContentNotFound");

            List<CategoryViewModel> categories = new List<CategoryViewModel>();

            // Generate Categories
            AdminHelpers.GenerateCategories(ref categories, _owinContext);
            ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

            return View(model);
        }

        // POST: Administration/Category/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditCategoryByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }

                List<CategoryViewModel> categories = new List<CategoryViewModel>();
                // Generate Categories
                AdminHelpers.GenerateCategories(ref categories, _owinContext);
                ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

                return View(model);
            }
            catch (Exception ex)
            {
                List<CategoryViewModel> categories = new List<CategoryViewModel>();
                // Generate Categories
                AdminHelpers.GenerateCategories(ref categories, _owinContext);
                ViewBag.Categories = categories.Select(m => new SelectListItem() { Value = m.CatCode, Text = m.Name });

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Category/Delete/5
        [HttpGet]
        public ActionResult Delete(string catCode)
        {
            CategoryViewModel model = _owinContext.GetCategoryViewModelByCatCode(catCode);
            if (model == null)
                throw new HttpException(404, "ContentNotFound");

            return View(model);
        }

        // POST: Administration/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string catCode)
        {
            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteCategoryByCatCode(catCode);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                CategoryViewModel model = _owinContext.GetCategoryViewModelByCatCode(catCode);
                if (model == null)
                    throw new HttpException(404, "ContentNotFound");

                return View(model);
            }
        }
    }
}
