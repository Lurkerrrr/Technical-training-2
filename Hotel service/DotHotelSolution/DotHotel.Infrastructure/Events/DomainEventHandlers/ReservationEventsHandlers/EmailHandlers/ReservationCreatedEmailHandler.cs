using DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents;
using DotHotel.Core.Domain.Events.DomainEventsHandlers;
using DotHotel.Core.Helpers;
using DotHotel.Core.ServiceContracts;

namespace DotHotel.Infrastructure.Events.DomainEventHandlers.ReservationEventsHandlers.EmailHandlers
{
    public class ReservationCreatedEmailHandler : DomainEventHandler<ReservationCreatedEvent>
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IReservationService _reservationService;

        public ReservationCreatedEmailHandler(IEmailSenderService emailSenderService, IReservationService reservationService)
        {
            _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        }

        public override async Task HandleAsync(ReservationCreatedEvent domainEvent)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(domainEvent.ReservationId);
            (var subject, var body) = EmailTemplates.ReservationCreated(reservation!.ReservationHolder.FirstName, reservation.Apartment.Name, reservation.CheckInDate, reservation.CheckOutDate);

            await _emailSenderService.SendEmailAsync(reservation.ReservationHolder.Email, subject, body);
        }
    }
}
