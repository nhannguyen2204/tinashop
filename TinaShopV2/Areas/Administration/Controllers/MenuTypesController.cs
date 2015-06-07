using System;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.MenuType;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class MenuTypesController : BaseController
    {
        public MenuTypesController()
            : base()
        {
            
        }

        // GET: Administration/MenuTypes
        public ActionResult Index()
        {
            var model = _owinContext.GetAllMenuTypeViewModel();
            return View(model);
        }

        // GET: Administration/MenuTypes/Details/5
        public ActionResult Details(int id)
        {
            var menuType = _dbContextService.MenuTypes.Find(id);
            if (menuType == null)
                throw new HttpException(404, "ContentNotFound");

            MenuTypeViewModel model = new MenuTypeViewModel(_owinContext);
            AutoMapper.Mapper.Map(menuType, model);
            
            return View(model);
        }

        // GET: Administration/MenuTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/MenuTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuTypeViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateMenuTypeByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Administration/MenuTypes/Edit/5
        public ActionResult Edit(int id)
        {
            var menuType = _dbContextService.MenuTypes.Find(id);
            if (menuType == null)
                throw new HttpException(404, "ContentNotFound");

            MenuTypeViewModel model = new MenuTypeViewModel(_owinContext);
            AutoMapper.Mapper.Map(menuType, model);

            return View(model);
        }

        // POST: Administration/MenuTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuTypeViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var menuType = _dbContextService.MenuTypes.Find(model.Id);
                    if (menuType == null)
                        throw new HttpException(400, "BadRequest");

                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditMenuTypeByViewModel(model);
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

        // GET: Administration/MenuTypes/Delete/5
        public ActionResult Delete(int id)
        {
            var menuType = _dbContextService.MenuTypes.Find(id);
            if (menuType == null)
                throw new HttpException(404, "ContentNotFound");

            MenuTypeViewModel model = new MenuTypeViewModel(_owinContext);
            AutoMapper.Mapper.Map(menuType, model);

            return View(model);
        }

        // POST: Administration/MenuTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var menuType = _dbContextService.MenuTypes.Find(id);
            if (menuType == null)
                throw new HttpException(400, "BadRequest");

            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteMenuTypeByViewModel(id);
                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                MenuTypeViewModel model = new MenuTypeViewModel(_owinContext);
                AutoMapper.Mapper.Map(menuType, model);

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
