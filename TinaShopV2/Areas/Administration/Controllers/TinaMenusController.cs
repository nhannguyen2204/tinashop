﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class TinaMenusController : BaseController
    {
        public TinaMenusController()
            : base()
        {

        }

        // GET: Administration/TinaMenus
        public ActionResult Index(int? menuTypeId)
        {
            var menuTypes = _dbContextService.MenuTypes.AsEnumerable();

            if (menuTypeId == null && menuTypes.Count() > 0)
            {
                menuTypeId = menuTypes.First().Id;
                return RedirectToAction("Index", "TinaMenus", new { @menuTypeId = menuTypeId });
            }
            else if (menuTypeId != null && !_dbContextService.MenuTypes.Any(m => m.Id == menuTypeId))
                throw new HttpException(404, "ContentNotFound");

            ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            IndexTinaMenuViewModel model = new IndexTinaMenuViewModel();
            model.MenuTypeId = menuTypeId;
            List<TinaMenuViewModel> tinaMenus = new List<TinaMenuViewModel>();
            AdminHelpers.GenerateTinaMenus(ref tinaMenus, _owinContext, menuTypeId ?? 0, null);
            model.TinaMenus = tinaMenus;

            foreach (var item in model.TinaMenus)
            {
                item.SetOwinContext(_owinContext);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        // GET: Administration/TinaMenus/Details/5
        public ActionResult Details(int id)
        {
            var tinaMenu = _dbContextService.TinaMenus.Find(id);
            if (tinaMenu == null)
                throw new HttpException(404, "ContentNotFound");

            TinaMenuViewModel model = new TinaMenuViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaMenu, model);

            return View(model);
        }

        // GET: Administration/TinaMenus/Create
        public ActionResult Create(int? menuTypeId)
        {
            var menuTypes = _dbContextService.MenuTypes.AsEnumerable();

            if (menuTypeId == null && menuTypes.Count() > 0)
            {
                menuTypeId = menuTypes.First().Id;
                return RedirectToAction("Create", "TinaMenus", new { @menuTypeId = menuTypeId });
            }
            else if (menuTypeId != null && !_dbContextService.MenuTypes.Any(m => m.Id == menuTypeId))
            {
                throw new HttpException(404, "ContentNotFound");
            }

            ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
            ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            return View(new TinaMenuViewModel(_owinContext) { MenuTypeId = menuTypeId ?? 0 });
        }

        // POST: Administration/TinaMenus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TinaMenuViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateTinaMenuByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }

                var menuTypes = _dbContextService.MenuTypes.AsEnumerable();
                ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
                ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

                return View(model);
            }
            catch (Exception ex)
            {
                var menuTypes = _dbContextService.MenuTypes.AsEnumerable();
                ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
                ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/TinaMenus/Edit/5
        public ActionResult Edit(int id)
        {
            var tinaMenu = _dbContextService.TinaMenus.Find(id);
            if (tinaMenu == null)
                throw new HttpException(404, "ContentNotFound");

            TinaMenuViewModel model = new TinaMenuViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaMenu, model);

            var menuTypes = _dbContextService.MenuTypes.AsEnumerable();
            ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
            ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

            return View(model);
        }

        // POST: Administration/TinaMenus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TinaMenuViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditTinaMenuByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index", new { @menuTypeId = model.MenuTypeId });
                }

                var menuTypes = _dbContextService.MenuTypes.AsEnumerable();
                ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
                ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

                return View(model);
            }
            catch (Exception ex)
            {
                var menuTypes = _dbContextService.MenuTypes.AsEnumerable();
                ViewBag.MenuTypes = menuTypes.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });
                ViewBag.Actions = _dbContextService.TinaActions.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name });

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/TinaMenus/Delete/5
        public ActionResult Delete(int id)
        {
            var tinaMenu = _dbContextService.TinaMenus.Find(id);
            if (tinaMenu == null)
                throw new HttpException(404, "ContentNotFound");

            TinaMenuViewModel model = new TinaMenuViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaMenu, model);

            return View(model);
        }

        // POST: Administration/TinaMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tinaMenu = _dbContextService.TinaMenus.Find(id);
            if (tinaMenu == null)
                throw new HttpException(404, "ContentNotFound");

            try
            {
                int menuTypeId = tinaMenu.MenuTypeId;
                // TODO: Add delete logic here
                _owinContext.DeleteTinaMenuById(id);
                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index", new { @menuTypeId = menuTypeId });
            }
            catch (Exception ex)
            {
                TinaMenuViewModel model = new TinaMenuViewModel(_owinContext);
                AutoMapper.Mapper.Map(tinaMenu, model);

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
