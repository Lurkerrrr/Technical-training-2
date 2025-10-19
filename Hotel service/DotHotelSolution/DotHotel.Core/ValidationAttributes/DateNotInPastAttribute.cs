using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DotHotel.Core.ValidationAttributes
{
    public class DateNotInPastAttribute : ValidationAttribute, IClientModelValidator
    {
        public DateNotInPastAttribute()
        {
            ErrorMessage = "{0} cannot be in the past";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dateValue = value as DateTime?;

            if (dateValue.HasValue && dateValue.Value.Date < DateTime.Today)
            {
                return new ValidationResult(
                    string.Format(ErrorMessage!, validationContext.DisplayName)
                );
            }

            return ValidationResult.Success!;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());

            if (!context.Attributes.ContainsKey("data-val"))
            {
                context.Attributes.Add("data-val", "true");
            }
            context.Attributes.Add("data-val-datenotinpast", errorMessage);
        }
    }
}
