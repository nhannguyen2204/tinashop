using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class RolesController : Controller
    {
        // GET: Administration/Roles
        public ActionResult Index()
        {
            var model = ApplicationDbContext.Instance.GetAllRoleViewModel();
            return View(model);
        }

        // GET: Administration/Roles/Details/5
        public ActionResult Details(string id)
        {
            var role = ApplicationDbContext.Instance.Roles.Find(id);

            if (role == null)
                throw new HttpException(404, "ContentNotFound");

            RoleViewModel model = new RoleViewModel();
            AutoMapper.Mapper.Map(role, model);
            return View(model);
        }

        // GET: Administration/Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext.Instance.CreateRoleByViewModel(model);
                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                    return RedirectToAction("Index", ControllerContext.LocalizeRouteValues());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var role = ApplicationDbContext.Instance.Roles.Find(id);

            if (role == null)
                throw new HttpException(404, "ContentNotFound");

            RoleViewModel roleViewModel = new RoleViewModel();
            AutoMapper.Mapper.Map(role, roleViewModel);
            return View(roleViewModel);
        }

        // POST: Administration/Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleViewModel model)
        {
            IdentityRole role = ApplicationDbContext.Instance.Roles.Find(model.Id);

            if (role == null)
                throw new HttpException(400, "BadRequest");

            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationDbContext.Instance.EditRoleByViewModel(model);
                    TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                    return RedirectToAction("Index", ControllerContext.LocalizeRouteValues());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/Roles/Delete/5
        public ActionResult Delete(string id)
        {
            var role = ApplicationDbContext.Instance.Roles.Find(id);

            if (role == null)
                throw new HttpException(404, "ContentNotFound");

            RoleViewModel roleViewModel = new RoleViewModel();
            AutoMapper.Mapper.Map(role, roleViewModel);
            return View(roleViewModel);
        }

        // POST: Administration/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            IdentityRole role = ApplicationDbContext.Instance.Roles.Find(id);

            if (role == null)
                throw new HttpException(400, "BadRequest");

            try
            {
                ApplicationDbContext.Instance.DeleteRoleByViewModel(id);
                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index", ControllerContext.LocalizeRouteValues());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(role);
            }
        }
    }
}
