using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Controllers;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Common;
using TinaShopV2.Models.Entity;
using TinaShopV2.Areas.Administration.Models.CustomControls;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class MediasController : BaseController
    {
        // GET: Administration/Medias
        public ActionResult Index(MediaIndexViewModel model)
        {
            int total = 0;
            model.Medias = ApplicationDbContext.Instance.GetMediaViewModels(out total, model.Page, model.TypeId, model.ProductCode);
            var mediaTypes = ApplicationDbContext.Instance.MediaTypes.AsEnumerable();
            ViewBag.MediaTypes = mediaTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            ViewBag.Pager = new PagerViewModel() { CurrentPage = model.Page, PageTotal = total };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Index")]
        public ActionResult SearchMedia(MediaIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TypeId != GlobalObjects.MediaType_ProductImage_Id)
                    model.ProductCode = string.Empty;

                return RedirectToAction("Index", "Medias", new { area = "Administration", @TypeId = model.TypeId, @ProductCode = model.ProductCode, @Page = 1 });
            }

            return null;
        }

        // GET: Administration/Medias/Details/5
        public ActionResult Details(int id)
        {
            MediaViewModel model = ApplicationDbContext.Instance.GetMediaViewModelById(id);
            return View(model);
        }

        // GET: Administration/Medias/Create
        public ActionResult Create()
        {
            var mediaTypes = ApplicationDbContext.Instance.MediaTypes.AsEnumerable();
            ViewBag.MediaTypes = mediaTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            return View();
        }

        // POST: Administration/Medias/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(MediaViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    ApplicationDbContext.Instance.CreateMediaByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var mediaTypes = ApplicationDbContext.Instance.MediaTypes.AsEnumerable();
            ViewBag.MediaTypes = mediaTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
            return View(model);
        }

        // GET: Administration/Medias/Edit/5
        public ActionResult Edit(int id)
        {
            var mediaTypes = ApplicationDbContext.Instance.MediaTypes.AsEnumerable();
            ViewBag.MediaTypes = mediaTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            MediaViewModel model = ApplicationDbContext.Instance.GetMediaViewModelById(id);
            return View(model);
        }

        // POST: Administration/Medias/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(MediaViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    ApplicationDbContext.Instance.EditMediaByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var mediaTypes = ApplicationDbContext.Instance.MediaTypes.AsEnumerable();
            ViewBag.MediaTypes = mediaTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
            return View(model);
        }

        // GET: Administration/Medias/Delete/5
        public ActionResult Delete(int id)
        {
            MediaViewModel model = ApplicationDbContext.Instance.GetMediaViewModelById(id);
            return View(model);
        }

        // POST: Administration/Medias/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ApplicationDbContext.Instance.DeleteMediaById(id);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var model = ApplicationDbContext.Instance.GetMediaViewModelById(id);
                return View(model);
            }
        }
    }
}
