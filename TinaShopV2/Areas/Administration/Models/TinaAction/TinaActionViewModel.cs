using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.TinaAction
{
    public class TinaActionViewModel : BaseViewModel
    {
        public TinaActionViewModel() : base() { }

        public TinaActionViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            
        }

        public int Id { get; set; }

        [Display(Name = "Name", ResourceType = typeof(App_GlobalResources.Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Area { get; set; }

        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Controller { get; set; }

        [MaxLength(150, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string Action { get; set; }

        public bool IsAnonymous { get; set; }

        
        private string[] roleIds;
        [Display(Name = "Roles", ResourceType = typeof(App_GlobalResources.Commons))]
        public string[] RoleIds
        {
            get { return roleIds; }
            set 
            { 
                roleIds = value;
                if (roleIds != null)
                    Roles = _dbContextService.Roles.Where(m => roleIds.Contains(m.Id));
            }
        }

        public IEnumerable<IdentityRole> Roles { get; private set; }

        public void LoadRoles()
        {
            var tinaAction = _dbContextService.TinaActions.Find(Id);
            if (tinaAction != null)
            {
                this.RoleIds = _dbContextService.TinaAuthorizes.Where(m => m.ActionId == this.Id).Select(m => m.RoleId).ToArray();
            }
        }

        //public virtual ICollection<TinaAuthorize> TinaAuthorizes { get; set; }
        //public virtual ICollection<TinaMenu> TinaMenus { get; set; }
    }
}