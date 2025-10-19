namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationCheckedOutEvent : ReservationBaseEvent
    {
        public ReservationCheckedOutEvent(Guid? reservationId) : base(reservationId)
        { }
    }
}
