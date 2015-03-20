using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

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
    }
}