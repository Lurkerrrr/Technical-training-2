using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.ServiceContracts;
using DotHotel.Presentation.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Controllers
{
    [Route("apartments")]
    public class ApartmentController : Controller
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        [TypeFilter(typeof(PassParametersToViewBagActionFilter), Arguments = new object[] { new[] { nameof(searchOptions) } })]
        public async Task<IActionResult> Index(ApartmentSearchOptions? searchOptions)
        {
            var apartments = await _apartmentService.GetApartmentsAsync(searchOptions ?? new ApartmentSearchOptions());
            return View(apartments);
        }

        [AllowAnonymous]
        [Route("details/{apartmentId:guid}")]
        [HttpGet]
        public async Task<IActionResult> ApartmentDetails(Guid apartmentId)
        {
            var apartment = await _apartmentService.GetApartmentByIdAsync(apartmentId);
            if (apartment == null)
            {
                throw new ArgumentException("Apartment not found.", nameof(apartmentId));
            }

            return View(apartment);
        }

        [Route("validate-guest-count")]
        [HttpGet]
        public async Task<IActionResult> ValidateGuestCount(int guestCount, Guid apartmentId)
        {
            var result = await _apartmentService.ValidateGuestCountAsync(apartmentId, guestCount);

            return Json(result);
        }
    }
}
