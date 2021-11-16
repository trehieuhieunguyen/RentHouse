using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace RentHouse.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string destination, string customerName, string htmlMessage)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("trehieu040520@gmail.com");
                mail.To.Add(destination);
                mail.Subject = "RentHouse";
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;
                using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("trehieuhieunguyen3@gmail.com", "hieu01685360016");
                smtp.EnableSsl = true;
                smtp.Timeout = 1000;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
