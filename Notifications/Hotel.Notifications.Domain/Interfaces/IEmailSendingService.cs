using MimeKit;

namespace Hotel.Notifications.Domain.Interfaces;

/// <summary>
/// Сервис для отправки электронных писем
/// </summary>
public interface IEmailSendingService
{
    /// <summary>
    /// Отправить электронное письмо
    /// </summary>
    /// <param name="message">Электронное письмо</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task SendEmailAsync(MimeMessage message);
}