using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;

namespace TinaShopV2.Areas.Administration.Models.Product
{
    public class ProductIndexViewModel : IndexBasicViewModel
    {
        [MaxLength(128, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Product", ResourceType = typeof(Commons))]
        public string ProductCode { get; set; }

        [Display(Name = "ProductName", ResourceType = typeof(Commons))]
        public string ProductName { get; set; }

        [Display(Name = "Brand", ResourceType = typeof(Commons))]
        public string BrandCode { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        public PublishStatus IsPublish { get; set; }

        [Display(Name = "Delete", ResourceType = typeof(Commons))]
        public DeleteStatus IsDeleted { get; set; }

        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        public SaleState CanSale { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }

    public enum PublishStatus
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        NoState = 0,
        [Display(Name = "Published", ResourceType = typeof(Commons))]
        Published = 1,
        [Display(Name = "NonPublished", ResourceType = typeof(Commons))]
        NonPublish = 2
    }

    public enum DeleteStatus
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        NoState = 0,
        [Display(Name = "Deleted", ResourceType = typeof(Commons))]
        Deleted = 1,
        [Display(Name = "NonDeleted", ResourceType = typeof(Commons))]
        NoDelete = 2
    }

    public enum SaleState
    {
        [Display(Name = "NoState", ResourceType = typeof(Commons))]
        NoState = 0,
        [Display(Name = "CanSale", ResourceType = typeof(Commons))]
        CanSale = 1,
        [Display(Name = "CanNotSale", ResourceType = typeof(Commons))]
        CanNotSale = 2
    }
}