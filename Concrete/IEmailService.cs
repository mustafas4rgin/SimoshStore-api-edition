namespace SimoshStore;

public interface IEmailService
{
    Task<IServiceResult> SendEmailAsync(string to, string subject, string body);
}
