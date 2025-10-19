using System.ComponentModel.DataAnnotations;
using DotHotel.Core.Domain.Models.HotelModels;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public abstract class ApartmentRequestBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per night must be greater than zero.")]
        public decimal PricePerNightPerPersonUsd { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        //[Required(ErrorMessage = "You have to upload at least one image")]
        //[DataType(DataType.Upload)]
        //[AllowedExtensions(Extensions = new string[] { "png", "jpg", "jpeg", "gif" })]
        //public List<IFormFile> ImageFiles { get; set; } = null!;

        [Required(ErrorMessage = "You have to add at least one image")]
        public List<string> ApartmentImagesBase64 { get; set; } = null!;

        public abstract Apartment ToApartment();
    }
}
