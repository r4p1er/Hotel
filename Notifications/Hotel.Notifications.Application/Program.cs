using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Hotel.Notifications.Domain;
using Hotel.Notifications.Domain.Models;
using Hotel.Notifications.Infrastructure;
using Hotel.Notifications.Infrastructure.Models;

namespace Hotel.Notifications.Application;

/// <summary/>
class Program
{
    /// <summary/>
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var notificationOptions = builder.Configuration.GetRequiredSection("Notification")
            .Get<NotificationServiceOptions>();
        builder.Services.AddNotifications(notificationOptions!);

        var sendingOptions = builder.Configuration.GetRequiredSection("Sending")
            .Get<EmailSendingServiceOptions>();
        builder.Services.AddEmailSending(sendingOptions!);

        builder.Services.AddRabbitMq(builder.Configuration["RabbitOptions:Host"]!);

        var host = builder.Build();
        await host.RunAsync();
    }
}