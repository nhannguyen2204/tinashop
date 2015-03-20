using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Areas.Administration.Models.Brand;
using TinaShopV2.Controllers;
using TinaShopV2.Common;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class BrandsController : BaseController
    {
        // GET: Administration/Brands
        public ActionResult Index()
        {
            var model = ApplicationDbContext.Instance.GetAllBrandViewModels();
            return View(model);
        }

        // GET: Administration/Brands/Details/5
        public ActionResult Details(string brandCode)
        {
            var model = ApplicationDbContext.Instance.GetBrandViewModelById(brandCode);
            return View(model);
        }

        // GET: Administration/Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Brands/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(BrandViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    ApplicationDbContext.Instance.CreateBrandByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Brands/Edit/5
        public ActionResult Edit(string brandCode)
        {
            var model = ApplicationDbContext.Instance.GetBrandViewModelById(brandCode);
            return View(model);
        }

        // POST: Administration/Brands/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(BrandViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    ApplicationDbContext.Instance.EditBrandByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Brands/Delete/5
        public ActionResult Delete(string brandCode)
        {
            var model = ApplicationDbContext.Instance.GetBrandViewModelById(brandCode);
            return View(model);
        }

        // POST: Administration/Brands/Delete/5
        [HttpPost,ValidateAntiForgeryToken,ActionName("Delete")]
        public ActionResult DeleteConfirmed(string brandCode)
        {
            try
            {
                // TODO: Add delete logic here
                ApplicationDbContext.Instance.DeleteBrandById(brandCode);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var model = ApplicationDbContext.Instance.GetBrandViewModelById(brandCode);
                return View(model);
            }
        }
    }
}
