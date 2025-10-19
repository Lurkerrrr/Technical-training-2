using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Controllers
{
    [Route("pricing")]
    [Authorize]
    public class PricingController : Controller
    {
        private readonly IPricingService _pricingService;
        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [Route("calculate-price")]
        [HttpPost]
        public async Task<IActionResult> CalculatePrice([FromBody] ReservationPriceCalculationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            var response = await _pricingService.CalculateTotalPriceAsync(request);
            return Ok(response);
        }
    }
}
