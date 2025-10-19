using DotHotel.Core.DataTransferObjects.ApartmentObjects;
using DotHotel.Core.ServiceContracts;
using DotHotel.Presentation.Attributes;
using DotHotel.Presentation.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("administrator/apartments")]
    [Authorize(Roles = "Administrator")]
    public class ApartmentController : Controller
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentController(IApartmentService apartmentService)
        {
            _apartmentService = apartmentService;
        }

        [Route("")]
        [HttpGet]
        [TypeFilter(typeof(PassParametersToViewBagActionFilter), Arguments = new object[] { new[] { nameof(searchOptions) } })]
        public async Task<IActionResult> Index(ApartmentSearchOptions? searchOptions)
        {
            var apartments = await _apartmentService.GetApartmentsAsync(searchOptions ?? new ApartmentSearchOptions());
            return View(apartments);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult CreateApartment()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(ReturnModelOnValidationFailActionFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateApartment([ReturnOnValidationFail] ApartmentAddRequest apartmentAddRequest)
        {
            var createdApartment = await _apartmentService.AddApartmentAsync(apartmentAddRequest);

            return RedirectToAction("ApartmentDetails", new { apartmentId = createdApartment.Id });
        }

        [Route("update/{apartmentId:guid}")]
        [HttpGet]
        public async Task<IActionResult> UpdateApartment(Guid apartmentId)
        {
            var apartment = await _apartmentService.GetApartmentByIdAsync(apartmentId);
            if (apartment == null)
            {
                throw new ArgumentException("Apartment not found.", nameof(apartmentId));
            }

            return View(apartment.ToPublicationUpdateRequest());
        }

        [Route("update/{apartmentId:guid}")]
        [HttpPost]
        [TypeFilter(typeof(ReturnModelOnValidationFailActionFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApartment(ApartmentUpdateRequest apartmentUpdateRequest)
        {
            var updatedApartment = await _apartmentService.UpdateApartmentAsync(apartmentUpdateRequest);
            if (updatedApartment == null)
            {
                throw new ArgumentException("Apartment not found.", nameof(apartmentUpdateRequest.Id));
            }

            return RedirectToAction("ApartmentDetails", new { apartmentId = updatedApartment.Id });
        }

        [Route("delete/{apartmentId:guid}")]
        [HttpGet]
        public async Task<IActionResult> DeleteApartment(Guid apartmentId)
        {
            var isDeleted = await _apartmentService.DeleteApartmentAsync(apartmentId);

            return RedirectToAction("Index");
        }
    }
}
