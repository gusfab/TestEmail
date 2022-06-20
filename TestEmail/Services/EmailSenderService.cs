using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;
using TestEmail.Models;

namespace TestEmail.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSenderService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest request)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", request.Email));
                message.Subject = request.Subject;
                message.Body = new TextPart("html") { Text = request.Body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpSettings.Server, 465, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
