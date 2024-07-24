using Hotel.Shared.MessageContracts;
using Identity.Domain.Interfaces;
using MassTransit;

namespace Identity.Infrastructure.RabbitConsumers;

public class SelectUserDataConsumer(IUserService userService) : IConsumer<SelectUserData>
{
    public async Task Consume(ConsumeContext<SelectUserData> context)
    {
        var user = await userService.GetById(context.Message.Id);

        await context.RespondAsync<UserDataResult>(new UserDataResult
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        });
    }
}