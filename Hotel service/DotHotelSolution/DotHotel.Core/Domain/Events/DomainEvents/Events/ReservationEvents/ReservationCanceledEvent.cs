namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationCanceledEvent : ReservationBaseEvent
    {
        public ReservationCanceledEvent(Guid? reservationId) : base(reservationId)
        { }
    }
}
