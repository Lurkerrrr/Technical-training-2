namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationConfirmedEvent : ReservationBaseEvent
    {
        public ReservationConfirmedEvent(Guid? reservationId) : base(reservationId)
        { }
    }
}
