using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Controllers;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Areas.Administration.Models.Slider;
using TinaShopV2.Common;

namespace TinaShopV2.Areas.Administration.Controllers
{
    public class SlidersController : BaseController
    {
        // GET: Administration/Sliders
        public ActionResult Index()
        {
            var model = _owinContext.GetAllSliderViewModels();
            return View(model);
        }

        // GET: Administration/Sliders/Details/5
        public ActionResult Details(int id)
        {
            var model = _owinContext.GetSliderById(id);
            return View(model);
        }

        // GET: Administration/Sliders/Create
        public ActionResult Create()
        {
            return View(new SliderViewModel());
        }

        // POST: Administration/Sliders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SliderViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateSliderByViewModel(model);

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

        // GET: Administration/Sliders/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _owinContext.GetSliderById(id);
            return View(model);
        }

        // POST: Administration/Sliders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SliderViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditSliderByViewModel(model);

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

        // GET: Administration/Sliders/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _owinContext.GetSliderById(id);
            return View(model);
        }

        // POST: Administration/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteSliderById(id);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var model = _owinContext.GetSliderById(id);
                return View(model);
            }
        }
    }
}
