namespace DotHotel.Core.DataTransferObjects.ReservationObjects
{
    public class ReservationPriceCalculationResponse
    {
        public decimal PricePerNight { get; set; }
        public int NumberOfNights { get; set; }
        public decimal TotalPriceUsd { get; set; }
        public string Currency { get; set; } = "USD";
    }
}
