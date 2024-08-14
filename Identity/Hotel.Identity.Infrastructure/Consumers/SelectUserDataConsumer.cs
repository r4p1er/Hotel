using Hotel.Identity.Domain.Abstractions;
using Hotel.Shared.MessageContracts;
using MassTransit;

namespace Hotel.Identity.Infrastructure.Consumers;

/// <summary>
/// Потребитель команды SelectUserData
/// </summary>
/// <param name="userService">Сервис для работы с пользователями</param>
public class SelectUserDataConsumer(IUserService userService) : IConsumer<SelectUserData>
{
    /// <summary>
    /// Потребить сообщение
    /// </summary>
    /// <param name="context">Контекст потребления сообщения</param>
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