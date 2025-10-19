using DotHotel.Core.DataTransferObjects.ReservationObjects;

namespace DotHotel.Core.ServiceContracts
{
    public interface IPricingService
    {
        Task<ReservationPriceCalculationResponse> CalculateTotalPriceAsync(ReservationPriceCalculationRequest calculationRequest);
    }
}
