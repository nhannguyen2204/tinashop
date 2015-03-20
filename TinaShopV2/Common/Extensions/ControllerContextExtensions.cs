using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;

namespace TinaShopV2.Common.Extensions
{
    public static class ControllerContextExtensions
    {
        public static RouteValueDictionary LocalizeRouteValues(this ControllerContext controllerContext, object newRouteValues = null)
        {
            RouteValueDictionary combinedRouteValues = new RouteValueDictionary();

            combinedRouteValues["language"] = (string)controllerContext.RouteData.Values["language"];
            combinedRouteValues["culture"] = (string)controllerContext.RouteData.Values["culture"];

            if (newRouteValues != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newRouteValues))
                    combinedRouteValues[descriptor.Name] = descriptor.GetValue(newRouteValues);
            }

            return combinedRouteValues;
        }
    }
}