namespace DotHotel.Core.ServiceContracts
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string? toEmail, string subject, string body);
    }
}
