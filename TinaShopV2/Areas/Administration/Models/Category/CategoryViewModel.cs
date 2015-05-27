using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Common.Attributes.Validation;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;

namespace TinaShopV2.Areas.Administration.Models.Category
{
    public class CategoryViewModel : BaseViewModel
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "CatCode", ResourceType = typeof(Commons))]
        public string CatCode { get; set; }

        [Display(Name = "CatParentCode", ResourceType = typeof(Commons))]
        [NotEqual("CatCode", ErrorMessageResourceName = "NotSelectThisValue", ErrorMessageResourceType = typeof(App_GlobalResources.Errors))]
        public string CatParentCode { get; set; }

        public CategoryViewModel GetCatParent()
        {
            CategoryViewModel parent = null;

            if (!string.IsNullOrEmpty(this.CatParentCode))
            {
                CategoryViewModel cat = ApplicationDbContext.Instance.GetCategoryViewModelByCatCode(this.CatParentCode);
                if (cat != null)
                    parent = cat;
            }

            return parent;
        }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Name", ResourceType = typeof(Commons))]
        public string Name { get; set; }

        [Display(Name = "OrderNumber", ResourceType = typeof(Commons))]
        public int OrderNumber { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        public bool IsPublish { get; set; }
    }
}