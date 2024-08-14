using Hotel.Booking.Domain.Entities;
using Hotel.Booking.Domain.Models;

namespace Hotel.Booking.Domain.Abstractions;

/// <summary>
/// Интерфейс сервиса для работы с заявками на бронирование
/// </summary>
public interface ITicketService
{
    /// <summary>
    /// Возвращает все существующие заявки на бронирование
    /// </summary>
    /// <returns>Коллекция с заявками на бронирование</returns>
    Task<IEnumerable<Ticket>> GetAll();
    
    /// <summary>
    /// Получить заявку на бронирование по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>Заявка на бронирование</returns>
    Task<Ticket> GetById(Guid id);
    
    /// <summary>
    /// Создать новую заявку на бронирование
    /// </summary>
    /// <param name="userId">Идентификатор пользователя, бронирующего номер</param>
    /// <param name="data">Данные для создания заявки на бронирование</param>
    /// <returns>Заявка на бронирование</returns>
    Task<Ticket> CreateTicket(Guid userId, TicketData data);
    
    /// <summary>
    /// Изменить статус заявки на бронирование на противоположный
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>Task, представляющий задачу по изменению статуса</returns>
    Task SwitchConfirmationStatus(Guid id);
    
    /// <summary>
    /// Отменить (удалить) заявку на бронирование по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>Task, представляющий задачу по отмене заявки</returns>
    Task CancelTicket(Guid id);
}