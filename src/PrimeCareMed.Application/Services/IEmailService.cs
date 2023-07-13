using PrimeCareMed.Application.Common.Email;

namespace PrimeCareMed.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
