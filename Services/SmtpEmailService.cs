using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using InvestmentPortfolioAPI.Models;

public class SmtpEmailService
{
    private readonly EmailSettings _emailSettings;

    public SmtpEmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        using (var client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort))
        {
            client.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
