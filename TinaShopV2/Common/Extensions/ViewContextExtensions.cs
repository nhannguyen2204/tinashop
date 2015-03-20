using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;

namespace TinaShopV2.Common.Extensions
{
    public static class ViewContextExtensions
    {
        public static RouteValueDictionary LocalizeRouteValues(this ViewContext viewContext, object newRouteValues = null)
        {
            RouteValueDictionary combinedRouteValues = new RouteValueDictionary();
            
            combinedRouteValues["language"] = (string)viewContext.RouteData.Values["language"];
            combinedRouteValues["culture"] = (string)viewContext.RouteData.Values["culture"];

            if (newRouteValues != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newRouteValues))
                    combinedRouteValues[descriptor.Name] = descriptor.GetValue(newRouteValues);
            }

            return combinedRouteValues;
        }
    }
}