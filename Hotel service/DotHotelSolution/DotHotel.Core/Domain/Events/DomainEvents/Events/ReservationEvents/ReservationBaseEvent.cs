namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationBaseEvent : IDomainEvent
    {
        public Guid ReservationId { get; set; }

        public ReservationBaseEvent(Guid? reservationId)
        {
            ReservationId = reservationId ?? throw new ArgumentNullException(nameof(reservationId));
        }
    }
}
