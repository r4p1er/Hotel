using MailKit.Net.Smtp;
using MimeKit;
using Hotel.Notifications.Domain.Interfaces;
using Hotel.Notifications.Infrastructure.Models;

namespace Hotel.Notifications.Infrastructure.Services;

/// <summary>
/// Сервис для отправки электронных писем
/// </summary>
/// <param name="options">Опции сервиса для отправки электронных писем</param>
public class EmailSendingService(EmailSendingServiceOptions options) : IEmailSendingService
{
    /// <summary>
    /// SMTP клиент, по которому отправляются электронные письма
    /// </summary>
    private readonly SmtpClient _client = new();
    
    /// <inheritdoc cref="IEmailSendingService.SendEmailAsync"/>
    public async Task SendEmailAsync(MimeMessage message)
    {
        if (!_client.IsConnected) await _client.ConnectAsync("smtp.gmail.com", 587, false);

        if (!_client.IsAuthenticated) await _client.AuthenticateAsync(options.Email, options.Password);

        await _client.SendAsync(message);
    }
}