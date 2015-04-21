using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using TinaShopV2.Areas.Administration.Models.CustomControls;
using TinaShopV2.Areas.Administration.Models.TinaMenu;
using TinaShopV2.Common.Extensions;
using TinaShopV2.Models;
using TinaShopV2.Common;
using TinaShopV2.Areas.Administration.Models.Product;
using TinaShopV2.Areas.Administration.Models;

namespace TinaShopV2.Common.Extensions
{
    public static class AdminHtmlHelperExtentions
    {
        #region TinaMenu DropdownList

        public static void GenerateTinaMenus(ref List<TinaMenuViewModel> models, int menuTypeId, int? parentId, int level = 0, bool? isHidden = null)
        {
            if (models == null)
                models = new List<TinaMenuViewModel>();

            var tinaMenus = ApplicationDbContext.Instance.GetTinaMenuViewModelByTypeAndParent(menuTypeId, parentId, isHidden);
            foreach (var item in tinaMenus)
            {
                string levelStr = string.Empty;
                for (int i = 0; i < level; i++)
                {
                    levelStr += "----------";
                }

                item.Name = string.Format("{0} {1}", levelStr, item.Name);
                models.Add(item);
                var menus = ApplicationDbContext.Instance.GetTinaMenuViewModelByTypeAndParent(menuTypeId, item.Id, isHidden);
                if (menus.Count() > 0)
                    GenerateTinaMenus(ref models, menuTypeId, item.Id, level + 1, isHidden);
            }
        }

        public static MvcHtmlString TinaMenuDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, int>> expressionMenuType, object htmlAttributes = null, bool isEnableLabel = false, bool? isHidden = null)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            string menuTypeIdStr = GetValue(htmlHelper, expressionMenuType);
            string menuIdStr = GetValue(htmlHelper, expression);
            if (!string.IsNullOrEmpty(menuTypeIdStr))
            {
                int menuTypeId = 0;
                if (int.TryParse(menuTypeIdStr, out menuTypeId))
                {
                    int menuId = 0;
                    int.TryParse(menuIdStr, out menuId);
                    List<TinaMenuViewModel> models = new List<TinaMenuViewModel>();
                    GenerateTinaMenus(ref models, menuTypeId, null, 0, isHidden);
                    selectList = models.Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Name, Selected = m.Id == menuId }).ToList();
                }
            }
            return htmlHelper.SmartDropDownListFor(expression, selectList, htmlAttributes, isEnableLabel);
        }

        #endregion

        #region DropdownList

        public static MvcHtmlString SmartDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, object htmlAttributes = null, bool isEnableLabel = false)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();

            if (htmlAttributes != null)
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //(SmartStyle) Add attribute with value 'select2'
            attributes = attributes.AddAttributes("class", "select2");

            return htmlHelper.DropDownListFor(expression, selectList, isEnableLabel ? string.Format(App_GlobalResources.Commons.SelectOptionLabelFormat, htmlHelper.DisplayNameFor(expression)) : null, attributes);
        }

        #endregion

        #region Listbox

        public static MvcHtmlString SmartListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();

            if (htmlAttributes != null)
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //(SmartStyle) Add attribute with value 'select2'
            attributes = attributes.AddAttributes("class", "select2");

            return htmlHelper.ListBoxFor(expression, selectList, attributes);
        }

        #endregion

        #region Enum

        public static MvcHtmlString SmartEnumListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool isEnableLabel = false)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();

            if (htmlAttributes != null)
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //(SmartStyle) Add attribute with value 'select2'
            attributes = attributes.AddAttributes("class", "select2");

            return isEnableLabel ? htmlHelper.EnumDropDownListFor(expression, string.Format(App_GlobalResources.Commons.SelectOptionLabelFormat, htmlHelper.DisplayNameFor(expression)), attributes) : htmlHelper.EnumDropDownListFor(expression, attributes);
        }

        #endregion

        #region Checkbox
        public static MvcHtmlString SmartCheckboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();

            if (htmlAttributes != null)
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            ////(SmartStyle) Add attribute with value 'checkbox style-0'
            //attributes = attributes.AddAttributes("class", "checkbox", "style-0");

            // Get value from expression
            string valueStr = GetValue(htmlHelper, expression);
            bool value = !string.IsNullOrEmpty(valueStr) && valueStr.ToLower() == "true" ? true : false;

            // Get name from expression
            string name = expression.MemberName();

            var model = new SmartCheckbox() { Name = name, Value = value, HtmlAttributes = attributes };

            return htmlHelper.Partial("CustomControls/_SmartCheckboxPartial", model);
        }
        #endregion

        #region Spinner

        public static MvcHtmlString SmartSpinner<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();

            if (htmlAttributes != null)
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            //(SmartStyle) Add attribute with value 'select2'
            attributes = attributes.AddAttributes("class", "form-control", "spinner-left");

            return htmlHelper.TextBoxFor(expression, attributes);
        }

        #endregion

        #region ValidationSummary

        public static MvcHtmlString SmartValidationSummary(this HtmlHelper htmlHelper)
        {
            MessageSummary model = new MessageSummary();

            var sTempMessages = htmlHelper.ViewContext.Controller.TempData[GlobalObjects.SuccesMessageKey];
            var eTempMessages = htmlHelper.ViewContext.Controller.TempData[GlobalObjects.ErrorMessageKey];
            var sMessages = htmlHelper.ViewBag.success_messages;
            var eMessages = htmlHelper.ViewBag.error_messages;

            List<object> successObjects = new List<object>() { sTempMessages, sMessages };
            foreach (var item in successObjects)
            {
                if (item != null && item is IEnumerable<string>)
                {
                    foreach (var message in (item as IEnumerable<string>))
                        model.Successes.Add(message);
                }
                else if (item != null && item is string && !string.IsNullOrEmpty(item as string))
                {
                    model.Successes.Add(item as string);
                }
            }

            List<object> errorObjects = new List<object>() { eTempMessages, eMessages };
            foreach (var item in errorObjects)
            {
                if (item != null && item is IEnumerable<string>)
                {
                    foreach (var message in (item as IEnumerable<string>))
                        model.Errors.Add(message);
                }
                else if (item != null && item is string && !string.IsNullOrEmpty(item as string))
                {
                    model.Errors.Add(item as string);
                }
            }

            // Add error from modelstate
            if (htmlHelper.ViewData.ModelState.Values != null && htmlHelper.ViewData.ModelState.Values.Count > 0)
            {
                foreach (var item in htmlHelper.ViewData.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                        model.Errors.Add(error.ErrorMessage);
                }
            }

            return htmlHelper.Partial("CustomControls/_SmartValidationSummaryPartial", model);
        }

        #endregion

        #region Private Methods

        private static string GetValue<TModel, TProperty>(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            string propertyName = body.Member.Name;
            TModel model = helper.ViewData.Model;

            if (helper.ViewData.Model == null)
                return null;

            var valueObject = typeof(TModel).GetProperty(propertyName).GetValue(model, null);
            return valueObject != null ? valueObject.ToString() : null;
        }

        private static string MemberName<T, V>(this Expression<Func<T, V>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression");

            return memberExpression.Member.Name;
        }

        #endregion

        #region Pager

        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, IndexBasicViewModel model)
        {
            if (model != null && model.PageTotal > 1 && model.Page > 0)
            {
                return htmlHelper.Partial("CustomControls/_PagerPartial", model);
            }
            return null;
        }

        #endregion
    }
}