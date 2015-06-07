using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.User;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class UsersController : BaseController
    {
        public UsersController()
            : base()
        {

        }

        // GET: Administration/UserViewModel
        public ActionResult Index()
        {
            var users = _dbContextService.Users.AsEnumerable().Select(m => new UserViewModel(_owinContext) { Id = m.Id, Email = m.Email, PhoneNumber = m.PhoneNumber, UserName = m.UserName, LockoutEnabled = m.LockoutEnabled });
            return View(users);
        }

        // GET: Administration/UserViewModel/Details/5
        public ActionResult Details(string id)
        {
            var user = _dbContextService.Users.Find(id);

            if (user == null)
                throw new HttpException(404, "ContentNotFound");

            UserViewModel userViewModel = new UserViewModel(_owinContext);
            AutoMapper.Mapper.Map(user, userViewModel);

            return View(userViewModel);
        }

        // GET: Administration/UserViewModel/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", null);
            return View();
        }

        // POST: Administration/UserViewModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var emailExisting = _userManagerService.Users.FirstOrDefault(m => m.Email.ToLower().Trim() == model.Email.ToLower().Trim());
                    if (emailExisting != null)
                    {
                        ModelState.AddModelError("", "Email's existing");
                        goto responseToClient;
                    }

                    var phoneNumberExisting = _userManagerService.Users.FirstOrDefault(m => m.PhoneNumber.ToLower().Trim() == model.PhoneNumber.ToLower().Trim());
                    if (phoneNumberExisting != null)
                    {
                        ModelState.AddModelError("", "Phone Number's existing");
                        goto responseToClient;
                    }

                    var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                    var result = await _userManagerService.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        foreach (var item in model.RoleId)
                        {
                            var role = _dbContextService.Roles.Find(item);
                            await _userManagerService.AddToRoleAsync(user.Id, role.Name);
                        }

                        TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.CreateSuccessMessage;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        AddErrors(result);
                }

                responseToClient:

                if (model.RoleId != null)
                {
                    var selectedRoles = _dbContextService.Roles.Where(m => model.RoleId.Contains(m.Id));
                    ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", selectedRoles);
                }
                else
                {
                    ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", new List<IdentityRole>());
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/UserViewModel/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManagerService.FindByIdAsync(id);

            if (user == null)
                throw new HttpException(404, "ContentNotFound");

            var model = new EditUserViewModel(_owinContext);
            AutoMapper.Mapper.Map(user, model);

            model.RoleId = user.Roles.Select(m => m.RoleId).ToArray();
            var selectedRoles = _dbContextService.Roles.Where(m => model.RoleId.Contains(m.Id));
            ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", selectedRoles);

            return View(model);
        }

        // POST: Administration/UserViewModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManagerService.FindByIdAsync(model.Id);

                    if (user == null)
                        throw new HttpException(404, "ContentNotFound");

                    var emailExisting = _userManagerService.Users.FirstOrDefault(m => m.Email.ToLower().Trim() == model.Email.ToLower().Trim() && m.Id != model.Id);
                    if (emailExisting != null)
                    {
#warning localize message
                        ModelState.AddModelError("", "Email's existing");
                        goto responseToClient;
                    }

                    var phoneNumberExisting = _userManagerService.Users.FirstOrDefault(m => m.PhoneNumber.ToLower().Trim() == model.PhoneNumber.ToLower().Trim() && m.Id != model.Id);
                    if (phoneNumberExisting != null)
                    {
#warning localize message
                        ModelState.AddModelError("", "Phone Number's existing");
                        goto responseToClient;
                    }

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    // Update user
                    var result = await _userManagerService.UpdateAsync(user);

                    // Remove all roles
                    var roles = await _userManagerService.GetRolesAsync(user.Id);
                    var removeRoleResult = await _userManagerService.RemoveFromRolesAsync(user.Id, roles.ToArray());

                    IdentityResult addRoleResult = null;
                    // Add new roles
                    if (model.RoleId != null && model.RoleId.Count() > 0)
                    {
                        string[] newRoles = _dbContextService.Roles.Where(m => model.RoleId.Contains(m.Id)).Select(m => m.Name).ToArray();
                        addRoleResult = await _userManagerService.AddToRolesAsync(user.Id, newRoles);
                    }


                    if (result.Succeeded && removeRoleResult.Succeeded && (addRoleResult == null || addRoleResult.Succeeded))
                    {
                        TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.UpdateSuccessMessage;
                        return RedirectToAction("Index");
                    }
                    else
                        AddErrors(result);
                }

                responseToClient:

                //var selectedRoles = _owinContext.Roles.Where(m => model.RoleId.Contains(m.Id));
                //ViewBag.Roles = new MultiSelectList(_owinContext.Roles.AsEnumerable(), "Id", "Name", selectedRoles);

                if (model.RoleId != null)
                {
                    var selectedRoles = _dbContextService.Roles.Where(m => model.RoleId.Contains(m.Id));
                    ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", selectedRoles);
                }
                else
                {
                    ViewBag.Roles = new MultiSelectList(_dbContextService.Roles.AsEnumerable(), "Id", "Name", new List<IdentityRole>());
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Administration/UserViewModel/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManagerService.FindByIdAsync(id);

            if (user == null)
                throw new HttpException(404, "ContentNotFound");

            var userViewModel = new UserViewModel(_owinContext);
            AutoMapper.Mapper.Map(user, userViewModel);

            return View(userViewModel);
        }

        // POST: Administration/UserViewModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManagerService.FindByIdAsync(id);

            if (user == null)
                throw new HttpException(400, "BadRequest");

            user.LockoutEnabled = true;
            var result = await _userManagerService.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            else
                AddErrors(result);

            var model = new UserViewModel(_owinContext);
            AutoMapper.Mapper.Map(user, model);

            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
