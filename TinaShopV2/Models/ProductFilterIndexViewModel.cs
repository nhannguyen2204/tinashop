using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinaShopV2.Areas.Administration.Models;
using TinaShopV2.Common;

namespace TinaShopV2.Models
{
    public class ProductFilterIndexViewModel : IndexBasicViewModel
    {
        public ProductFilterIndexViewModel()
            : base()
        {
            InitModel();
        }

        public ProductFilterIndexViewModel(IOwinContext owinContext)
            : base(owinContext)
        {
            InitModel();
        }

        public ProductFilterIndexViewModel(ProductFilterIndexViewModel model)
        {
            // properties of BaseOwinContext
            this._dbContextService = model._dbContextService;
            this._owinContext = model._owinContext;
            this._userManagerService = model._userManagerService;

            // properties of IndexBasicViewModel
            this.Page = model.Page;
            this.PageTotal = model.PageTotal;
            this.Total = model.Total;

            // properties
            this.FromPrice = model.FromPrice;
            this.ToPrice = model.ToPrice;
            this.ColorKeys = model.ColorKeys;
            this.CatCode = model.CatCode;
            this.BrandCode = model.BrandCode;
            this.IsOrderDatetimeDesc = model.IsOrderDatetimeDesc;
            this.IsOrderPriceDesc = model.IsOrderPriceDesc;
            this.Products = new List<ProductFOViewModel>(model.Products);
        }

        private void InitModel()
        {
            Products = new List<ProductFOViewModel>();
            FromPrice = 0;
            ToPrice = 3000;
            ColorKeys = GlobalObjects.DefaultAllColors;
            CatCode = GlobalObjects.DefaultAllCatCode;
            BrandCode = GlobalObjects.DefaultAllBrandCode;
        }

        private int fromPrice = 0;
        public int FromPrice
        {
            get { return fromPrice; }
            set { fromPrice = value; }
        }

        private int toPrice = 3000;
        public int ToPrice
        {
            get { return toPrice; }
            set { toPrice = value; }
        }

        private string colorKeys = GlobalObjects.DefaultAllColors;
        public string ColorKeys
        {
            get { return colorKeys; }
            set { colorKeys = value; }
        }

        private string catCode = GlobalObjects.DefaultAllCatCode;
        public string CatCode
        {
            get { return catCode; }
            set { catCode = value; }
        }

        private string brandCode = GlobalObjects.DefaultAllBrandCode;
        public string BrandCode
        {
            get { return brandCode; }
            set { brandCode = value; }
        }

        public int IsOrderDatetimeDesc { get; set; }
        public int IsOrderPriceDesc { get; set; }

        public List<ProductFOViewModel> Products { get; set; }
    }
}