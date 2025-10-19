using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DotHotel.Core.ValidationAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute, IClientModelValidator
    {
        public string[] Extensions { get; set; } = new string[] { "png", "jpg" };

        private const string DefaultErrorMessage = "The file extension is not allowed. Allowed extensions: {0}";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (!Extensions.Contains(file.FileName.Split('.').Last().ToLower()))
                    {
                        return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, string.Join(", ", Extensions)));
                    }
                }

                return ValidationResult.Success;
            }

            return null;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
            {
                context.Attributes.Add("data-val", "true");
            }
            context.Attributes.Add("data-val-filesextensions", string.Format(ErrorMessage ?? DefaultErrorMessage, string.Join(", ", Extensions)));
            context.Attributes.Add("data-val-filesextensions-extensions", string.Join(",", Extensions));
        }
    }
}
