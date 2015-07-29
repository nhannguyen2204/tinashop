using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TinaShopV2.Models;

namespace TinaShopV2.Common
{
    public static class Extentions
    {
        #region IDictionary<string,object> Extentions

        public static RouteValueDictionary AddAttributes(this RouteValueDictionary attributes, string attName, params string[] values)
        {
            if (string.IsNullOrEmpty(attName))
                return attributes;

            if (attributes == null)
                attributes = new RouteValueDictionary();

            string classAtt = string.Empty;
            if (attributes.ContainsKey(attName))
                classAtt = attributes[attName] as string;

            if (!string.IsNullOrEmpty(classAtt))
                classAtt = classAtt.ToLower();

            char[] splitKeys = { ' ' };
            string[] classAtts = classAtt.Split(splitKeys, StringSplitOptions.RemoveEmptyEntries);
            if (values != null && values.Count() > 0)
            {
                foreach (var item in values)
                {
                    if (!classAtts.Contains(item))
                    {
                        classAtt += string.Format(" {0}", item);
                        classAtt = classAtt.Trim();
                    }
                }
            }

            attributes[attName] = classAtt;

            return attributes;
        }

        #endregion

        #region FO / Product Listing

        public static string GetUrlByModel(this UrlHelper urlHelper, ProductFilterIndexViewModel model)
        {
            if (model == null)
                return string.Empty;

            string url = urlHelper.Action("Index", "Products", new
            {
                @Page = model.Page,
                @FromPrice = model.FromPrice,
                @ToPrice = model.ToPrice,
                @ColorKeys = model.ColorKeys,
                @CatCode = model.CatCode,
                @BrandCode = model.BrandCode,
                @IsOrderPriceDesc = model.IsOrderPriceDesc,
                @IsOrderDatetimeDesc = model.IsOrderDatetimeDesc
            });

            return HttpUtility.UrlDecode(url);
        }

        #endregion
    }
}