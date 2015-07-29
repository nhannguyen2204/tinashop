using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TinaShopV2.App_GlobalResources;
using TinaShopV2.Areas.Administration.Models.Media;
using TinaShopV2.Models;
using TinaShopV2.Common.Extensions;

namespace TinaShopV2.Areas.Administration.Models.Catalog
{
    public class CatalogViewModel : BaseViewModel
    {
        public CatalogViewModel()
            : base()
        {
            InitModel();
        }

        public CatalogViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            InitModel();
        }

        private void InitModel()
        {
            this.Products = new List<TinaShopV2.Models.Entity.Product>();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Name", ResourceType = typeof(Commons))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        [Display(Name = "Image", ResourceType = typeof(Commons))]
        public Nullable<int> MediaId { get; set; }

        [Display(Name = "Publish", ResourceType = typeof(Commons))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Errors))]
        public bool IsPublished { get; set; }

        [Display(Name = "Product", ResourceType = typeof(Commons))]
        public virtual ICollection<TinaShopV2.Models.Entity.Product> Products { get; set; }
        [Display(Name = "Product", ResourceType = typeof(Commons))]
        public string ProductIds { get; set; }

        public MediaViewModel GetImage()
        {
            MediaViewModel image = null;
            if (MediaId != null)
                image = _owinContext.GetMediaViewModelById(MediaId.Value);

            return image;
        }
    }
}