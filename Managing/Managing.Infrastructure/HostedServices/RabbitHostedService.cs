using System.Text.Json;
using Hotel.Shared.Interfaces;
using Hotel.Shared.Models;
using Managing.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Managing.Infrastructure.HostedServices;

public class RabbitHostedService(IRabbitService rabbit, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        rabbit.Listen(async message =>
        {
            if (message.Name == "RoomsNameRequired")
            {
                var roomsId = JsonSerializer.Deserialize<List<Guid>>(message.Data, serializerOptions);
                var result = new List<object>();

                using (var scope = scopeFactory.CreateScope())
                {
                    var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
                    
                    foreach (var id in roomsId!)
                    {
                        var room = await roomService.GetById(id);
                        result.Add(new { Id = id, Name = room.Summary });
                    }
                }

                rabbit.SendMessage(new RabbitMessage
                {
                    Id = Guid.NewGuid(),
                    Name = "RoomsNameResponse",
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