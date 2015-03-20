using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace TinaShopV2.Areas.Administration.Models.CustomControls
{
    public class SmartCheckbox
    {
        public string Name { get; set; }
        public bool Value { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
    }
}