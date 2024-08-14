using Hotel.Shared.MessageContracts;

namespace Hotel.Notifications.Domain.Abstractions;

/// <summary>
/// Сервис для уведомления пользователей по электронной почте
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Уведомить о создании новой заявки на бронирование
    /// </summary>
    /// <param name="publishedEvent">Опубликованное событие из других микросервисов</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task TicketCreateNotify(BookingTicketCreated publishedEvent);

    /// <summary>
    /// Уведомить об отмене заявки на бронирование
    /// </summary>
    /// <param name="publishedEvent">Опубликованное событие из других микросервисов</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task TicketCancelNotify(BookingTicketCanceled publishedEvent);

    /// <summary>
    /// Уведомить об изменении статуса заявки на бронирование
    /// </summary>
    /// <param name="publishedEvent">Опубликованное событие из других микросервисов</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task StatusChangeNotify(ConfirmationStatusChanged publishedEvent);
}