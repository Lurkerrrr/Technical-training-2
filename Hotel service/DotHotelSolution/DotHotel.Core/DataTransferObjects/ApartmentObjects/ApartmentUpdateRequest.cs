using System.ComponentModel.DataAnnotations;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Helpers;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public class ApartmentUpdateRequest : ApartmentRequestBase
    {
        [Required(ErrorMessage = "Apartment ID is required.")]
        public Guid Id { get; set; }

        public override Apartment ToApartment()
        {
            return new Apartment
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Rating = Rating,
                Capacity = Capacity,
                PricePerNightPerPersonUsd = PricePerNightPerPersonUsd,
                IsActive = IsActive,
                ApartmentImages = ImagesConverterHelper.ConvertBase64ImagesToByteArrays(ApartmentImagesBase64)
                    .Select(imageBytes => new ApartmentImage
                    {
                        ImageData = imageBytes
                    })
                    .ToList()
            };
        }
    }

    public static class ApartmentUpdateRequestExtensions
    {
        public static ApartmentUpdateRequest ToPublicationUpdateRequest(this ApartmentResponse request)
        {
            return new ApartmentUpdateRequest
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Rating = request.Rating,
                Capacity = request.Capacity,
                PricePerNightPerPersonUsd = request.PricePerNightPerPersonUsd,
                IsActive = request.IsActive,
                ApartmentImagesBase64 = request.ApartmentImagesBase64
            };
        }
    }
}
