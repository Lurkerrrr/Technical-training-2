using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Helpers;

namespace DotHotel.Core.DataTransferObjects.ApartmentObjects
{
    public class ApartmentAddRequest : ApartmentRequestBase
    {
        public override Apartment ToApartment()
        {
            return new Apartment
            {
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
}
