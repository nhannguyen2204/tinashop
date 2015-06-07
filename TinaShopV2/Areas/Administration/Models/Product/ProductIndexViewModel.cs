using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Models;

namespace TinaShopV2.Areas.Administration.Models.Product
{
    public class ProductIndexViewModel : IndexBasicViewModel
    {
        public ProductIndexViewModel() : base() { }

        public ProductIndexViewModel(IOwinContext owinContext)
            : base(owinContext)
        {

        }

        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "ProductCode", ResourceType = typeof(Commons))]
        public string ProductCode { get; set; }

        [Display(Name = "Brand", ResourceType = typeof(Commons))]
        public string BrandCode { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        public int IsPublish { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Commons))]
        public int IsDeleted { get; set; }

        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        public int CanSale { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }

    public enum PublishStatus
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        Null,
        [Display(Name = "Published", ResourceType = typeof(Commons))]
        Published,
        [Display(Name = "NonPublished", ResourceType = typeof(Commons))]
        NonPublish
    }

    public enum DeleteStatus
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        Null,
        [Display(Name = "Deleted", ResourceType = typeof(Commons))]
        Deleted,
        [Display(Name = "NonDeleted", ResourceType = typeof(Commons))]
        NoDelete
    }

    public enum SaleState
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        Null,
        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        CanSale,
        [Display(Name = "CanNotSale", ResourceType = typeof(Commons))]
        CanNotSale
    }
}