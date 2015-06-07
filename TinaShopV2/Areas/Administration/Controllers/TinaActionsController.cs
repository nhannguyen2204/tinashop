using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.TinaAction;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class TinaActionsController : BaseController
    {
        public TinaActionsController()
            : base()
        {

        }

        // GET: Administration/TinaActions
        public ActionResult Index()
        {
            var model = _owinContext.GetAllTinaActionViewModel();
            return View(model);
        }

        // GET: Administration/TinaActions/Details/5
        public ActionResult Details(int id)
        {
            var tinaAction = _dbContextService.TinaActions.Find(id);
            if (tinaAction == null)
                throw new HttpException(404, "ContentNotFound");

            TinaActionViewModel model = new TinaActionViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaAction, model);
            model.LoadRoles();

            return View(model);
        }

        // GET: Administration/TinaActions/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", null);
            return View();
        }

        // POST: Administration/TinaActions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TinaActionViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateTinaActionByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index");
                }

                ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", model.RoleIds);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", model.RoleIds);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/TinaActions/Edit/5
        public ActionResult Edit(int id)
        {
            var tinaAction = _dbContextService.TinaActions.Find(id);
            if (tinaAction == null)
                throw new HttpException(404, "ContentNotFound");

            TinaActionViewModel model = new TinaActionViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaAction, model);
            model.LoadRoles();

            ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", model.RoleIds);
            return View(model);
        }

        // POST: Administration/TinaActions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TinaActionViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var existingItem = _dbContextService.TinaActions.Find(model.Id);
                    if (existingItem == null)
                        throw new HttpException(400, "BadRequest");

                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditTinaActionByViewModel(model);

                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index");
                }

                ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", model.RoleIds);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", model.RoleIds);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/TinaActions/Delete/5
        public ActionResult Delete(int id)
        {
            var tinaAction = _dbContextService.TinaActions.Find(id);
            if (tinaAction == null)
                throw new HttpException(404, "ContentNotFound");

            TinaActionViewModel model = new TinaActionViewModel(_owinContext);
            AutoMapper.Mapper.Map(tinaAction, model);
            return View(model);
        }

        // POST: Administration/TinaActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var model = _dbContextService.TinaActions.Find(id);
            if (model == null)
                throw new HttpException(400, "BadRequest");

            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteTinaActionByViewModel(id);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
