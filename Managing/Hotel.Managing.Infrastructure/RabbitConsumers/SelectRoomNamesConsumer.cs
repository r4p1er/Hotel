using Hotel.Shared.MessageContracts;
using Hotel.Shared.Models;
using Hotel.Managing.Domain.Interfaces;
using MassTransit;

namespace Hotel.Managing.Infrastructure.RabbitConsumers;

/// <summary>
/// Потребитель команды SelectRoomNames
/// </summary>
/// <param name="roomService">Сервис для работы с номерами отеля</param>
public class SelectRoomNamesConsumer(IRoomService roomService) : IConsumer<SelectRoomNames>
{
    /// <summary>
    /// Потребить сообщение
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<SelectRoomNames> context)
    {
        var result = new List<RoomData>();

        foreach (var id in context.Message.Guids)
        {
            var room = await roomService.GetById(id);
            
            result.Add(new RoomData
            {
                Id = id,
                Name = room.Summary
            });
        }

        await context.RespondAsync<RoomNamesResult>(new RoomNamesResult { Rooms = result });
    }
}