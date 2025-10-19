namespace DotHotel.Core.Domain.Models.HotelModels
{
    public class ApartmentImage
    {
        public Guid Id { get; set; }
        public byte[] ImageData { get; set; } = null!;

        public Guid ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;
    }
}
