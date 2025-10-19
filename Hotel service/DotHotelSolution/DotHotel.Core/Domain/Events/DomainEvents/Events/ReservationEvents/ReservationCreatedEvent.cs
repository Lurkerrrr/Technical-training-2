namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationCreatedEvent : ReservationBaseEvent
    {
        public ReservationCreatedEvent(Guid? reservationId) : base(reservationId)
        { }
    }
}
