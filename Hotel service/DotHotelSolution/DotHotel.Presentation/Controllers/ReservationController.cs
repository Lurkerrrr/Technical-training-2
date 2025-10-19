using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents;
using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.ServiceContracts;
using DotHotel.Presentation.Attributes;
using DotHotel.Presentation.BackgroundServices;
using DotHotel.Presentation.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Controllers
{
    [Route("reservations")]
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IApartmentService _apartmentService;

        private readonly DomainEventBackgroundService _domainEventBackgroundService;

        public ReservationController(IReservationService reservationService, UserManager<ApplicationUser> userManager, IApartmentService apartmentService, DomainEventBackgroundService domainEventDispatcher)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _apartmentService = apartmentService ?? throw new ArgumentNullException(nameof(apartmentService));
            _domainEventBackgroundService = domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));
        }

        [Route("")]
        [Route("my-reservations")]
        [HttpGet]
        public async Task<IActionResult> Index(ReservationSearchOptions? searchOptions)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            searchOptions ??= new ReservationSearchOptions();
            searchOptions.Filter ??= new ReservationFilter();

            searchOptions.Filter.ReservationHolderId = user.Id;

            var reservations = await _reservationService.GetReservationsAsync(searchOptions);
            return View(reservations);
        }

        [Route("details/{reservationId:guid}")]
        [HttpGet]
        public async Task<IActionResult> ReservationDetails(Guid reservationId)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
            if (reservation == null)
            {
                throw new ArgumentException("Reservation not found.", nameof(reservationId));
            }

            return View(reservation);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> CreateReservation([FromQuery] Guid apartmentId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            if (apartmentId == Guid.Empty)
            {
                throw new ArgumentException("Apartment ID cannot be empty.", nameof(apartmentId));
            }

            var apartment = await _apartmentService.GetApartmentByIdAsync(apartmentId);
            if (apartment == null)
            {
                throw new ArgumentException("Apartment not found.", nameof(apartmentId));
            }

            var reservationAddRequest = new ReservationAddRequest()
            {
                ApartmentId = apartmentId,
                ReservationHolderId = user.Id,
                Apartment = apartment
            };

            return View(reservationAddRequest);
        }

        [Route("create")]
        [HttpPost]
        [TypeFilter(typeof(ReturnModelOnValidationFailActionFilter))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation([ReturnOnValidationFail] ReservationAddRequest reservationAddRequest)
        {
            var result = await _reservationService.ValidateReservationAddRequestAsync(reservationAddRequest);

            if (!result)
            {
                return View(reservationAddRequest);
            }

            var createdReservation = await _reservationService.CreateReservationAsync(reservationAddRequest);

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationCreatedEvent(createdReservation.Id));

            return RedirectToAction("ReservationDetails", new { reservationId = createdReservation.Id });
        }

        [Route("cancel/{reservationId:guid}")]
        [HttpGet]
        public async Task<IActionResult> CancelReservation(Guid reservationId)
        {
            var isCancelled = await _reservationService.CancelReservationAsync(reservationId);
            if (!isCancelled)
            {
                throw new ArgumentException("Reservation cancellation failed.", nameof(reservationId));
            }

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationCanceledEvent(reservationId));

            return RedirectToAction("Index");
        }

        [Route("process-payment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(ReservationPaymentRequest reservationPaymentRequest)
        {
            var isPaid = await _reservationService.ProcessReservationPaymentAsync(reservationPaymentRequest);
            if (!isPaid)
            {
                throw new ArgumentException("Payment process failed.", nameof(isPaid));
            }

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationConfirmedEvent(reservationPaymentRequest.ReservationId));

            return RedirectToAction("Index");
        }

        [Route("validate-reservation-dates")]
        [HttpGet]
        public async Task<IActionResult> ValidateReservationDates(DateTime checkInDate, DateTime checkOutDate, Guid apartmentId)
        {
            var result = await _reservationService.ValidateReservationDatesAsync(apartmentId, checkInDate, checkOutDate);

            return Json(result);
        }
    }
}
