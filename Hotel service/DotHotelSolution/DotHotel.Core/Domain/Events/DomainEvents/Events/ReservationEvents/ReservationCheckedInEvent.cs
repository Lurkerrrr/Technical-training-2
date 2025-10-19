namespace DotHotel.Core.Domain.Events.DomainEvents.Events.ReservationEvents
{
    public class ReservationCheckedInEvent : ReservationBaseEvent
    {
        public ReservationCheckedInEvent(Guid? reservationId) : base(reservationId)
        { }
    }
}
