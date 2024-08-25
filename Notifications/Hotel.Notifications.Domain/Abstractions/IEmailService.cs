using MimeKit;

namespace Hotel.Notifications.Domain.Abstractions;

/// <summary>
/// Сервис для отправки электронных писем
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправить электронное письмо
    /// </summary>
    /// <param name="message">Электронное письмо</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task SendEmailAsync(MimeMessage message);
}