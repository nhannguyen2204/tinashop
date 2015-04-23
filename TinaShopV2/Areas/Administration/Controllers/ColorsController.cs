using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.Color;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Controllers;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class ColorsController : BaseController
    {
        // GET: Administration/Colors
        public ActionResult Index()
        {
            var model = ApplicationDbContext.Instance.GetAllColorViewModels();
            return View(model);
        }

        // GET: Administration/Colors/Details/5
        public ActionResult Details(string colorKey)
        {
            var model = ApplicationDbContext.Instance.GetColorByKey(colorKey);
            return View(model);
        }

        // GET: Administration/Colors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Colors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ColorViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    ApplicationDbContext.Instance.CreateColorByViewModel(model);

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

        // GET: Administration/Colors/Edit/5
        public ActionResult Edit(string colorKey)
        {
            var model = ApplicationDbContext.Instance.GetColorByKey(colorKey);
            return View(model);
        }

        // POST: Administration/Colors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ColorViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    ApplicationDbContext.Instance.EditColorByViewModel(model);

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

        // GET: Administration/Colors/Delete/5
        public ActionResult Delete(string colorKey)
        {
            var model = ApplicationDbContext.Instance.GetColorByKey(colorKey);
            return View(model);
        }

        // POST: Administration/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string colorKey)
        {
            try
            {
                // TODO: Add delete logic here
                ApplicationDbContext.Instance.DeleteColorByKey(colorKey);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var model = ApplicationDbContext.Instance.GetColorByKey(colorKey);
                return View(model);
            }
        }
    }
}
