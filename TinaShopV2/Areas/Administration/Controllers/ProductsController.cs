﻿using System;
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

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class ProductsController : BaseController
    {
        public JsonResult Find(string productCode)
        {
            IEnumerable<ProductViewModel> result = null;

            if (!string.IsNullOrEmpty(productCode))
                result = ApplicationDbContext.Instance.FindProductViewModelById(productCode);

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Administration/Products
        public ActionResult Index(ProductIndexViewModel model)
        {
            model.Products = ApplicationDbContext.Instance.GetAllProductViewModels();


            ViewBag.Brands = ApplicationDbContext.Instance.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
            return View(model);
        }

        // GET: Administration/Products/Details/5
        public ActionResult Details(string productCode)
        {
            var model = ApplicationDbContext.Instance.GetProductViewModelById(productCode);
            return View(model);
        }

        // GET: Administration/Products/Create
        public ActionResult Create()
        {
            ViewBag.Brands = ApplicationDbContext.Instance.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
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
                    ApplicationDbContext.Instance.CreateProductByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Brands = ApplicationDbContext.Instance.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
            return View(model);
        }

        // GET: Administration/Products/Edit/5
        public ActionResult Edit(string productCode)
        {
            var model = ApplicationDbContext.Instance.GetProductViewModelById(productCode);

            ViewBag.Brands = ApplicationDbContext.Instance.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
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
                    model.SetInteractionUser(CurrentUser.Id);
                    ApplicationDbContext.Instance.EditProductByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Brands = ApplicationDbContext.Instance.GetAllBrandViewModels().Select(m => new SelectListItem() { Value = m.BrandCode.ToString(), Text = m.Name });
            return View(model);
        }

        // GET: Administration/Products/Delete/5
        public ActionResult Delete(string productCode)
        {
            var model = ApplicationDbContext.Instance.GetProductViewModelById(productCode);
            return View(model);
        }

        // POST: Administration/Products/Delete/5
        [HttpPost,ValidateAntiForgeryToken,ActionName("Delete")]
        public ActionResult DeleteConfirmed(string productCode)
        {
            var model = ApplicationDbContext.Instance.GetProductViewModelById(productCode);
            
            try
            {
                ApplicationDbContext.Instance.DeleteProductById(productCode);
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