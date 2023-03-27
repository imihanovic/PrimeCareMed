using System.Threading.Tasks;
using BookIt.Application.Common.Email;
using BookIt.Application.Services;

namespace BookIt.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
