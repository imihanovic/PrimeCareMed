using BookIt.Application.Common.Email;

namespace BookIt.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
