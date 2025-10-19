using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DotHotel.Core.ValidationAttributes
{
    public class MinimumDaysAfterAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _comparisonProperty;
        private readonly int _minimumDaysAfter;

        public MinimumDaysAfterAttribute(string comparisonProperty, int minimumDaysAfter = 1)
        {
            _comparisonProperty = comparisonProperty;
            _minimumDaysAfter = minimumDaysAfter;
            ErrorMessage = $"{{0}} must be at least {minimumDaysAfter} day(s) after {{1}}";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = value as DateTime?;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException($"Property '{_comparisonProperty}' not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (currentValue.HasValue && comparisonValue.HasValue)
            {
                var minimumDate = comparisonValue.Value.AddDays(_minimumDaysAfter);

                if (currentValue.Value < minimumDate)
                {
                    var currentDisplayName = validationContext.DisplayName;

                    var comparisonDisplayName = property.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault()?.Name ?? _comparisonProperty;

                    return new ValidationResult(
                        string.Format(ErrorMessage!, currentDisplayName, comparisonDisplayName)
                    );
                }
            }

            return ValidationResult.Success!;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var currentDisplayName = context.ModelMetadata.GetDisplayName();

            var comparisonProperty = context.ModelMetadata.ContainerType.GetProperty(_comparisonProperty);

            if (comparisonProperty == null)
                throw new ArgumentException($"Property '{_comparisonProperty}' not found");

            var comparisonDisplayName = comparisonProperty
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? _comparisonProperty;

            var errorMessage = string.Format(ErrorMessage!, currentDisplayName, comparisonDisplayName);

            if (!context.Attributes.ContainsKey("data-val"))
            {
                context.Attributes.Add("data-val", "true");
            }
            context.Attributes.Add("data-val-minimumdaysafter", errorMessage);
            context.Attributes.Add("data-val-minimumdaysafter-other", _comparisonProperty);
            context.Attributes.Add("data-val-minimumdaysafter-days", _minimumDaysAfter.ToString());
        }
    }

}
