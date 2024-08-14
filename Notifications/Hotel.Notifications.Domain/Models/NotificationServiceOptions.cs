namespace Hotel.Notifications.Domain.Models;

/// <summary>
/// Опции сервиса NotificationService
/// </summary>
public class NotificationServiceOptions
{
    /// <summary>
    /// Название почтового ящика
    /// </summary>
    public string MailboxName { get; set; }
    
    /// <summary>
    /// Адрес электронной почты
    /// </summary>
    public string Email { get; set; }
}