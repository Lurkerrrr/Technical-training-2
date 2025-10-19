using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DotHotel.Core.ValidationAttributes
{
    public class RequiredWithAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _otherProperty;

        public RequiredWithAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
            ErrorMessage = "If {1} is specified, {0} is also required";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherProperty == null)
                throw new ArgumentException($"Property '{_otherProperty}' not found");

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherValue != null && value == null)
            {
                var otherDisplayName = otherProperty.GetCustomAttribute<DisplayAttribute>()?.Name ?? _otherProperty;
                return new ValidationResult(
                    string.Format(ErrorMessage!, validationContext.DisplayName, otherDisplayName)
                );
            }

            return ValidationResult.Success!;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var currentDisplayName = context.ModelMetadata.GetDisplayName();
            var comparisonProperty = context.ModelMetadata.ContainerType.GetProperty(_otherProperty);
            if (comparisonProperty == null)
                throw new ArgumentException($"Property '{_otherProperty}' not found");

            var comparisonDisplayName = comparisonProperty
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? _otherProperty;

            var errorMessage = string.Format(ErrorMessage!, currentDisplayName, comparisonDisplayName);

            if (!context.Attributes.ContainsKey("data-val"))
            {
                context.Attributes.Add("data-val", "true");
            }
            context.Attributes.Add("data-val-requiredwith", errorMessage);
            context.Attributes.Add("data-val-requiredwith-other", _otherProperty); // Просто имя свойства
        }
    }
}
