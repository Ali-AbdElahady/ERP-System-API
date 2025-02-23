using ERP_System.Service.Helpers;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ERP_System.Service.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendResetLinkAsync(string email, string token)
        {
            var resetLink = $"https://localhost:7063/reset-password?token={token}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Password Reset";

            message.Body = new TextPart("html")
            {
                Text = $"<p>Click <a href='{resetLink}'>here</a> to reset your password. the token is {token}</p>"
            };

            try 
            {
                #region old
                    //using var client = new SmtpClient();
                    //await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, _emailSettings.UseSsl);
                    //await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
                    //await client.SendAsync(message);
                    //await client.DisconnectAsync(true);
                #endregion

                #region new
                    using var client = new SmtpClient();

                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    
                    await client.AuthenticateAsync("ali.test.202100@gmail.com", "lusu hzde rvpm moty");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                #endregion
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
