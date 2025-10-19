using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Helpers;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public class ApartmentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public int Capacity { get; set; }
        public decimal PricePerNightPerPersonUsd { get; set; }
        public bool IsActive { get; set; }

        public List<string> ApartmentImagesBase64 { get; set; } = new List<string>();
    }

    public static class ApartmentResponseExtensions
    {
        public static ApartmentResponse ToApartmentResponse(this Apartment apartment)
        {
            return new ApartmentResponse
            {
                Id = apartment.Id,
                Name = apartment.Name,
                Description = apartment.Description,
                Rating = apartment.Rating,
                Capacity = apartment.Capacity,
                PricePerNightPerPersonUsd = apartment.PricePerNightPerPersonUsd,
                IsActive = apartment.IsActive,
                ApartmentImagesBase64 = ImagesConverterHelper.ConvertByteArraysToBase64Images(apartment.ApartmentImages.Select(img => img.ImageData).ToList())
            };
        }
    }
}
