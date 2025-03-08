using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace SimoshStore;

public class SmtpConfiguration
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}

public class SmtpEmailService : IEmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";  
    private readonly int _smtpPort = 587;  
    private readonly string _username = "simoshstoreco@gmail.com";  
    private readonly string _password = "lnqr khna jkbx ffyq";  

    public async Task<IServiceResult> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_username),
                Subject = subject,
                Body = body,
                IsBodyHtml = false 
            };

            mailMessage.To.Add(to);

            using (var smtpClient = new SmtpClient(_smtpServer))
            {
                smtpClient.Port = _smtpPort;
                smtpClient.Credentials = new NetworkCredential(_username, _password);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 60000;  
                await smtpClient.SendMailAsync(mailMessage);
                return new ServiceResult(true, "E-posta başarıyla gönderildi.");
            }
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"{ex.Message}{ex.StackTrace}{ex.InnerException?.Message}");
        }
    }
}

