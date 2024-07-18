using System.Text.Json;
using Booking.Domain.Interfaces;
using Booking.Infrastructure.Models;
using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Booking.Infrastructure.HostedServices;

public class RabbitHostedService(IRabbitService rabbit, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        rabbit.Listen(async message =>
        {
            if (message.Name == "BookingDataRequired")
            {
                var dates = JsonSerializer.Deserialize<BookingDataRequiredModel>(message.Data, serializerOptions);
                var result = new List<object>();

                using (var scope = scopeFactory.CreateScope())
                {
                    var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                    var tickets = await ticketService.GetAll();
                    tickets = tickets.Where(x =>
                            x.From.Date >= dates!.From.Date && x.To.Date <= dates.To.Date && x.ConfirmationStatus)
                        .ToList();

                    foreach (var ticket in tickets)
                    {
                        result.Add(new
                        {
                            RoomId = ticket.RoomId,
                            From = ticket.From,
                            To = ticket.To
                        });
                    }
                }

                rabbit.SendMessage(new RabbitMessage
                {
                    Id = Guid.NewGuid(),
                    Name = "BookingDataResponse",
                    Receiver = message.ResponseTarget,
                    Data = JsonSerializer.Serialize(result)
                });
            }
        });

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        
        rabbit.Close();
    }
}