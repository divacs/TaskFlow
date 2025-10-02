
namespace TaskFlow.Utility.Interface
{
    public interface IEmailService
    {
        // Sends a confirmation email for register
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
