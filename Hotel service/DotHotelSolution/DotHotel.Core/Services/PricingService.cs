using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.RepositoryContracts;
using DotHotel.Core.ServiceContracts;

namespace DotHotel.Core.Services
{
    public class PricingService : IPricingService
    {
        private readonly IApartmentRepository _apartmentRepository;

        public PricingService(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository ?? throw new ArgumentNullException(nameof(apartmentRepository));
        }

        public async Task<ReservationPriceCalculationResponse> CalculateTotalPriceAsync(ReservationPriceCalculationRequest calculationRequest)
        {
            var apartment = await _apartmentRepository.GetApartmentByIdAsync(calculationRequest.ApartmentId);
            if (apartment == null)
            {
                throw new ArgumentException($"Apartment with ID {calculationRequest.ApartmentId} not found.", nameof(calculationRequest.ApartmentId));
            }

            var guestCount = calculationRequest.GuestCount;
            var numberOfNights = (calculationRequest.CheckOutDate - calculationRequest.CheckInDate).Days;

            var pricePerNightPerPerson = apartment.PricePerNightPerPersonUsd;

            if (numberOfNights <= 0 || guestCount <= 0 || pricePerNightPerPerson <= 0)
            {
                throw new ArgumentException("Invalid calculation parameters. Ensure that guest count, number of nights, and price per night are greater than zero.");
            }

            var pricePerNight = pricePerNightPerPerson * guestCount;
            var totalPrice = guestCount * numberOfNights * pricePerNightPerPerson;

            return new ReservationPriceCalculationResponse
            {
                NumberOfNights = numberOfNights,
                PricePerNight = pricePerNight,
                TotalPriceUsd = totalPrice,
                Currency = "USD"
            };
        }
    }
}
