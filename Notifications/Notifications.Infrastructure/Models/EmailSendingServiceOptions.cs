namespace Notifications.Infrastructure.Models;

/// <summary>
/// Опции сервиса для отправки электронных писем
/// </summary>
public class EmailSendingServiceOptions
{
    /// <summary>
    /// Адрес электронной почты, с которго идет отправка
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Пароль от электронной почты
    /// </summary>
    public string Password { get; set; }
}