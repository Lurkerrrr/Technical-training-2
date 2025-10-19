namespace DotHotel.Core.Domain.Models.HotelModels
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rating { get; set; } = 0;
        public int Capacity { get; set; }
        public decimal PricePerNightPerPersonUsd { get; set; }
        public bool IsActive { get; set; }

        public virtual List<ApartmentImage> ApartmentImages { get; set; } = new List<ApartmentImage>();
        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
