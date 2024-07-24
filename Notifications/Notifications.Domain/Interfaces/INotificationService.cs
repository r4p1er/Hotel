using Hotel.Shared.MessageContracts;

namespace Notifications.Domain.Interfaces;

public interface INotificationService
{
    Task TicketCreateNotify(BookingTicketCreated publishedEvent);

    Task TicketCancelNotify(BookingTicketCanceled publishedEvent);

    Task StatusChangeNotify(ConfirmationStatusChanged publishedEvent);
}