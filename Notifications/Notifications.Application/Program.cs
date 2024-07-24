using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Notifications.Domain;
using Notifications.Domain.Models;
using Notifications.Infrastructure;
using Notifications.Infrastructure.Models;

namespace Notifications.Application;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var notificationOptions = builder.Configuration.GetRequiredSection("Notification")
            .Get<NotificationServiceOptions>();
        builder.Services.AddNotifications(notificationOptions!);

        var sendingOptions = builder.Configuration.GetRequiredSection("Sending")
            .Get<EmailSendingServiceOptions>();
        builder.Services.AddEmailSending(sendingOptions!);

        builder.Services.AddRabbitMq(builder.Configuration["Rabbit:Host"]!);

        var host = builder.Build();
        await host.RunAsync();
    }
}