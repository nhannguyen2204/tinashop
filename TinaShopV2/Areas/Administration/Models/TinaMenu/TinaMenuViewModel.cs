using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Common.Attributes.Validation;
using TinaShopV2.Models;
using Microsoft.Owin;

namespace TinaShopV2.Areas.Administration.Models.TinaMenu
{
    public class TinaMenuViewModel : BaseViewModel
    {
        public TinaMenuViewModel()
            : base()
        {

        }

        public TinaMenuViewModel(IOwinContext owinContext)
            : base(owinContext)
        {

        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Editable(false)]
        [Display(Name = "MenuType", ResourceType = typeof(App_GlobalResources.Commons))]
        public int MenuTypeId { get; set; }

        private TinaShopV2.Models.Entity.MenuType menuTypeObj;
        public TinaShopV2.Models.Entity.MenuType MenuTypeObj
        {
            get
            {
                if (menuTypeObj == null)
                    menuTypeObj = _dbContextService.MenuTypes.Find(MenuTypeId);

                return menuTypeObj;
            }
        }

        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Name", ResourceType = typeof(App_GlobalResources.Commons))]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "CssClass", ResourceType = typeof(App_GlobalResources.Commons))]
        public string CssClass { get; set; }

        [Display(Name = "Action")]
        public Nullable<int> ActionId { get; set; }

        private TinaShopV2.Models.Entity.TinaAction actionObj;
        public TinaShopV2.Models.Entity.TinaAction ActionObj
        {
            get
            {
                if (ActionId != null)
                    actionObj = _dbContextService.TinaActions.Find(ActionId.Value);

                return actionObj;
            }
        }

        [NotEqual("Id", ErrorMessageResourceName = "NotSelectThisValue", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Parent", ResourceType = typeof(App_GlobalResources.Commons))]
        public Nullable<int> ParentId { get; set; }

        private TinaShopV2.Models.Entity.TinaMenu parentObj;
        public TinaShopV2.Models.Entity.TinaMenu ParentObj
        {
            get
            {
                if (ParentId != null)
                    parentObj = _dbContextService.TinaMenus.Find(ParentId.Value);

                return parentObj;
            }
        }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Display(Name = "Hidden", ResourceType = typeof(App_GlobalResources.Commons))]
        public bool IsHidden { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Range(0, int.MaxValue)]
        [Display(Name = "OrderNumber", ResourceType = typeof(App_GlobalResources.Commons))]
        public int OrderNumber { get; set; }


        public List<TinaMenuViewModel> GetChildrens(bool? isHidden = null)
        {
            IEnumerable<TinaShopV2.Models.Entity.TinaMenu> childrens;
            if (isHidden == null)
                childrens = _dbContextService.TinaMenus.Where(m => m.ParentId == Id && m.MenuTypeId == MenuTypeId);
            else
                childrens = _dbContextService.TinaMenus.Where(m => m.ParentId == Id && m.MenuTypeId == MenuTypeId && m.IsHidden == isHidden.Value);

            List<TinaMenuViewModel> model = new List<TinaMenuViewModel>();
            AutoMapper.Mapper.Map(childrens, model);
            return model;
        }
    }

    public class IndexTinaMenuViewModel
    {
        [Display(Name = "MenuType", ResourceType = typeof(App_GlobalResources.Commons))]

        public int? MenuTypeId { get; set; }
        public IEnumerable<TinaMenuViewModel> TinaMenus { get; set; }
    }
}