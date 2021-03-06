﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Areas.Administration.Models.Address;
using TinaShopV2.Controllers;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Common;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class AddressesController : BaseController
    {
        public AddressesController()
            : base()
        {

        }

        // GET: Administration/Addresses
        public ActionResult Index()
        {
            var model = _owinContext.GetAllAddressViewModels();
            return View(model);
        }

        // GET: Administration/Addresses/Details/5
        public ActionResult Details(int id)
        {
            var model = _owinContext.GetAddressViewModelById(id);
            return View(model);
        }

        // GET: Administration/Addresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Addresses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddressViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id, true);
                    _owinContext.CreateAddressByViewModel(model);

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

        // GET: Administration/Addresses/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _owinContext.GetAddressViewModelById(id);
            return View(model);
        }

        // POST: Administration/Addresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddressViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    model.SetInteractionUser(CurrentUser.Id);
                    _owinContext.EditAddressByViewModel(model);

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

        // GET: Administration/Addresses/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _owinContext.GetAddressViewModelById(id);
            return View(model);
        }

        // POST: Administration/Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                _owinContext.DeleteAddressById(id);

                TempData[GlobalObjects.SuccesMessageKey] = App_GlobalResources.Commons.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var model = _owinContext.GetAddressViewModelById(id);
                return View(model);
            }
        }
    }
}
