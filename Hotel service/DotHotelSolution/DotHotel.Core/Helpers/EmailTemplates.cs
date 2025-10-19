namespace DotHotel.Core.Helpers
{
    public static class EmailTemplates
    {
        public static (string Subject, string Body) ReservationCreated(string firstName, string apartmentName, DateTime checkIn, DateTime checkOut)
        {
            string subject = "Reservation Confirmation";
            string body = $@"
            <h3>Hello {firstName}!</h3>
            <p>Your reservation for <b>{apartmentName}</b> has been successfully created.</p>
            <p>Check-in: {checkIn:MMMM dd, yyyy}<br/>
               Check-out: {checkOut:MMMM dd, yyyy}</p>
            <p>We look forward to your stay!</p>";

            return (subject, body);
        }

        public static (string Subject, string Body) ReservationCancelled(string firstName, string apartmentName)
        {
            string subject = "Reservation Cancelled";
            string body = $@"
            <h3>Hello {firstName}!</h3>
            <p>Your reservation for <b>{apartmentName}</b> has been cancelled.</p>
            <p>If you have questions, contact us anytime.</p>";

            return (subject, body);
        }

        public static (string Subject, string Body) PaymentProcessed(string firstName, string apartmentName, decimal totalPrice)
        {
            string subject = "Payment Received";
            string body = $@"
            <h3>Hello {firstName}!</h3>
            <p>Payment for your reservation <b>{apartmentName}</b> has been received.</p>
            <p>Amount: <b>${totalPrice:F2}</b></p>";

            return (subject, body);
        }

        public static (string Subject, string Body) CheckedIn(string firstName, string apartmentName)
        {
            string subject = "Check-In Confirmed";
            string body = $@"
            <h3>Hello {firstName}!</h3>
            <p>You have successfully checked in to <b>{apartmentName}</b>. Enjoy your stay!</p>";

            return (subject, body);
        }

        public static (string Subject, string Body) CheckedOut(string firstName, string apartmentName)
        {
            string subject = "Check-Out Completed";
            string body = $@"
            <h3>Hello {firstName}!</h3>
            <p>Thank you for staying at <b>{apartmentName}</b>. We hope to see you again!</p>";

            return (subject, body);
        }
    }

}
