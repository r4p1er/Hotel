using Hotel.Notifications.Domain.Abstractions;
using MailKit.Net.Smtp;
using MimeKit;
using Hotel.Notifications.Infrastructure.Models;

namespace Hotel.Notifications.Infrastructure.Services;

/// <inheritdoc cref="IEmailSendingService"/>
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