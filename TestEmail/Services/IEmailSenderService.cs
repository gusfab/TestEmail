using System.Threading.Tasks;
using TestEmail.Models;

namespace TestEmail.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(MailRequest request);
    }
}
