using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace TinaShopV2.Common
{
    public class AdminHelpers
    {

    }

    public static class EnumHelper<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames()
        {
            return typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues()
        {
            return GetNames().Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;

            var resourceType = descriptionAttributes[0].ResourceType;
            var name = descriptionAttributes[0].Name;
            if (resourceType != null)
            {
                if (typeof(TinaShopV2.App_GlobalResources.Commons).Equals(resourceType))
                    return Helpers.GetResxNameByValue_Commons(name);

                if (typeof(TinaShopV2.App_GlobalResources.Errors).Equals(resourceType))
                    return Helpers.GetResxNameByValue_Errors(name);
            }

            return (descriptionAttributes.Length > 0) ? name : value.ToString();
        }

        public static IEnumerable<SelectListItem> EnumToList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            IList<string> names = GetDisplayValues();

            for (int i = 0; i < names.Count; i++)
            {
                list.Add(new SelectListItem() { Value = i.ToString(), Text = names[i] });
            }

            //var values = Enum.GetValues(typeof(T)).Cast<T>().Select(e => new { Id = Convert.ToInt32(e), Name = e.GetDescription() });

            return list;
        }
    }
}