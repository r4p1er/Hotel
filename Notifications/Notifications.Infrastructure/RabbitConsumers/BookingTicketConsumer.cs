using Hotel.Shared.MessageContracts;
using MassTransit;
using Notifications.Domain.Interfaces;

namespace Notifications.Infrastructure.RabbitConsumers;

public class BookingTicketConsumer(INotificationService notificationService) : IConsumer<BookingTicketCreated>, 
    IConsumer<BookingTicketCanceled>, 
    IConsumer<ConfirmationStatusChanged>
{
    public async Task Consume(ConsumeContext<BookingTicketCreated> context)
    {
        await notificationService.TicketCreateNotify(context.Message);
    }

    public async Task Consume(ConsumeContext<BookingTicketCanceled> context)
    {
        await notificationService.TicketCancelNotify(context.Message);
    }

    public async Task Consume(ConsumeContext<ConfirmationStatusChanged> context)
    {
        await notificationService.StatusChangeNotify(context.Message);
    }
}