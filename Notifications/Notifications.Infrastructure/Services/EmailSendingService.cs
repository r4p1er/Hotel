using MailKit.Net.Smtp;
using MimeKit;
using Notifications.Domain.Interfaces;
using Notifications.Infrastructure.Models;

namespace Notifications.Infrastructure.Services;

public class EmailSendingService(EmailSendingServiceOptions options) : IEmailSendingService
{
    private readonly SmtpClient _client = new();
    
    public async Task SendEmailAsync(MimeMessage message)
    {
        if (!_client.IsConnected) await _client.ConnectAsync("smtp.gmail.com", 587, false);

        if (!_client.IsAuthenticated) await _client.AuthenticateAsync(options.Email, options.Password);

        await _client.SendAsync(message);
    }
}