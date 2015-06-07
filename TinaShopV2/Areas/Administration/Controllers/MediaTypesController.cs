using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Areas.Administration.Models.MediaType;
using TinaShopV2.Controllers;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class MediaTypesController : BaseController
    {
        public MediaTypesController()
            : base()
        {

        }

        // GET: Administration/MediaTypes
        public ActionResult Index()
        {
            var model = _owinContext.GetAllMediaTypeViewModels();
            return View(model);
        }

        // GET: Administration/MediaTypes/Details/5
        public ActionResult Details(int id)
        {
            var model = _owinContext.GetMediaTypeViewModelById(id);
            return View(model);
        }

        // GET: Administration/MediaTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/MediaTypes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(MediaTypeViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateMediaTypeByViewModel(model);

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

        // GET: Administration/MediaTypes/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _owinContext.GetMediaTypeViewModelById(id);
            return View(model);
        }

        // POST: Administration/MediaTypes/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(MediaTypeViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditMediaTypeByViewModel(model);

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

        // GET: Administration/MediaTypes/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _owinContext.GetMediaTypeViewModelById(id);
            return View(model);
        }

        // POST: Administration/MediaTypes/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteMediaTypeByViewModel(id);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var model = _owinContext.GetMediaTypeViewModelById(id);
                return View(model);
            }
        }
    }
}
