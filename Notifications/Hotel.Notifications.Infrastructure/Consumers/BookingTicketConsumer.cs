using Hotel.Notifications.Domain.Abstractions;
using Hotel.Shared.MessageContracts;
using MassTransit;

namespace Hotel.Notifications.Infrastructure.Consumers;

/// <summary>
/// Потребитель событий о создании, отмене заявок на бронирование, а также об изменении статуса бронирования
/// </summary>
/// <param name="notificationService">Сервис для отправки уведомлений пользователям по электронной почте</param>
public class BookingTicketConsumer(INotificationService notificationService) : 
    IConsumer<BookingTicketCreated>, 
    IConsumer<BookingTicketCanceled>, 
    IConsumer<ConfirmationStatusChanged>
{
    /// <summary>
    /// Потребить событие о создании заявки на бронирование
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<BookingTicketCreated> context)
    {
        await notificationService.TicketCreateNotify(context.Message);
    }

    /// <summary>
    /// Потребить событие об отмене заявки на бронирование
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<BookingTicketCanceled> context)
    {
        await notificationService.TicketCancelNotify(context.Message);
    }

    /// <summary>
    /// Потребить событие об изменении статуса бронирования
    /// </summary>
    /// <param name="context">Контекст сообщения</param>
    public async Task Consume(ConsumeContext<ConfirmationStatusChanged> context)
    {
        await notificationService.StatusChangeNotify(context.Message);
    }
}