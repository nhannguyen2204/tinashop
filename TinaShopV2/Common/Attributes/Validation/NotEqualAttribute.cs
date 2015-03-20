using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TinaShopV2.App_GlobalResources;

namespace TinaShopV2.Common.Attributes.Validation
{
    public class NotEqualAttribute : ValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; private set; }
        public NotEqualAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(OtherProperty);
            if (property == null)
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0} is unknown property", OtherProperty));

            var otherValue = property.GetValue(validationContext.ObjectInstance, null);
            if (object.Equals(value, otherValue))
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "notequalto",
            };

            if (string.IsNullOrEmpty(ErrorMessage) && ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
                rule.ErrorMessage = (string)(HttpContext.GetGlobalResourceObject(ErrorMessageResourceType.Name, ErrorMessageResourceName));

            rule.ValidationParameters["other"] = OtherProperty;
            yield return rule;
        }
    }
}