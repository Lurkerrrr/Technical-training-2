using System.Net;
using System.Net.Mail;
using DotHotel.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;

namespace DotHotel.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailSenderService(IConfiguration configuration)
        {
            _smtpHost = configuration["Smtp:Host"] ?? throw new ArgumentNullException("Smtp:Host");
            _smtpPort = int.Parse(configuration["Smtp:Port"] ?? throw new ArgumentNullException("Smtp:Port"));
            _smtpUsername = configuration["Smtp:Username"] ?? throw new ArgumentNullException("Smtp:Username");
            _smtpPassword = configuration["Smtp:Password"] ?? throw new ArgumentNullException("Smtp:Password");

            _fromEmail = configuration["Smtp:FromEmail"] ?? "noreply@yourdomain.com";
            _fromName = configuration["Smtp:FromName"] ?? "DotHotel";
        }

        public async Task SendEmailAsync(string? toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                return;
            }

            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
