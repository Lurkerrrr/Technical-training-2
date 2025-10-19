using DotHotel.Core.DataTransferObjects.ReservationObjects;
using DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents;
using DotHotel.Core.Helpers;
using DotHotel.Core.ServiceContracts;
using DotHotel.Presentation.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Route("administrator/reservations")]
    [Authorize(Roles = "Administrator")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly DomainEventBackgroundService _domainEventBackgroundService;

        public ReservationController(IReservationService reservationService, DomainEventBackgroundService domainEventDispatcher)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _domainEventBackgroundService = domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index(ReservationSearchOptions? searchOptions)
        {
            var reservations = await _reservationService.GetReservationsAsync(searchOptions ?? new ReservationSearchOptions());
            return View(reservations);
        }

        [Route("confirm-cancel/{reservationId:guid}")]
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

        [Route("confirm-payment")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(ReservationPaymentRequest reservationPaymentRequest)
        {
            var isPaid = await _reservationService.ProcessReservationPaymentAsync(reservationPaymentRequest);
            if (!isPaid)
            {
                throw new ArgumentException("Payment process failed.", nameof(isPaid));
            }

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationCanceledEvent(reservationPaymentRequest.ReservationId));

            return RedirectToAction("Index");
        }

        [Route("check-in/{reservationId:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckIn(Guid reservationId)
        {
            var checkedIn = await _reservationService.ConfirmReservationCheckInAsync(reservationId);
            if (!checkedIn)
            {
                throw new ArgumentException("Check in process failed.", nameof(checkedIn));
            }

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationCheckedInEvent(reservationId));

            return RedirectToAction("Index");
        }

        [Route("check-out/{reservationId:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckOut(Guid reservationId)
        {
            var checkedOut = await _reservationService.ConfirmReservationCheckOutAsync(reservationId);
            if (!checkedOut)
            {
                throw new ArgumentException("Check out failed.", nameof(checkedOut));
            }

            var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
            var (subject, body) = EmailTemplates.CheckedOut(
                reservation!.ReservationHolder.FirstName,
                reservation!.Apartment.Name
            );

            await _domainEventBackgroundService.EnqueueEventAsync(new ReservationCheckedOutEvent(reservationId));

            return RedirectToAction("Index");
        }
    }
}
