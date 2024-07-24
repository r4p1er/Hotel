using Hotel.Shared.MessageContracts;
using Hotel.Shared.Models;
using Managing.Domain.Interfaces;
using MassTransit;

namespace Managing.Infrastructure.RabbitConsumers;

public class SelectRoomNamesConsumer(IRoomService roomService) : IConsumer<SelectRoomNames>
{
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