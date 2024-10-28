using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Maqalety.Code
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smptClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(
                   "mennanary@gmail.com", "rnnm cwgq qjjz whkq")
            };
            return smptClient.SendMailAsync("mennanary@gmail.com", email, subject, htmlMessage);
        }
    }
}
