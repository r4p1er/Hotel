using MimeKit;

namespace Notifications.Domain.Interfaces;

public interface IEmailSendingService
{
    Task SendEmailAsync(MimeMessage message);
}