using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Common;
using TinaShopV2.Common.Attributes;
using TinaShopV2.Controllers;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Controllers
{
    [TinaAdminAuthorization]
    public class TinaSystemsController : BaseController
    {
        public PartialViewResult MainMenu(int? parentId = null)
        {
            var tinaMenus = ApplicationDbContext.Instance.TinaMenus.Where(m => m.ParentId == parentId && m.MenuTypeId == AdminGlobalObjects.MainMenuTypeId).OrderBy(m => m.OrderNumber);
            List<TinaMenuViewModel> model = new List<TinaMenuViewModel>();
            AutoMapper.Mapper.Map(tinaMenus, model);
            return PartialView("_MainMenuPartial", model);
        }
    }
}