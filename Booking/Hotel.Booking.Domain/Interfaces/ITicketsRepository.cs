using Hotel.Booking.Domain.Entities;

namespace Hotel.Booking.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория с заявками на бронирование
/// </summary>
public interface ITicketsRepository
{
    /// <summary>
    /// Получить все заявки на бронирование
    /// </summary>
    /// <returns>IQueryable с заявками на бронирование</returns>
    IQueryable<Ticket> FindAll();
    
    /// <summary>
    /// Получить заявку на бронирование по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>Заявка на бронирования. Null - если не найдено</returns>
    Task<Ticket?> FindByIdAsync(Guid id);
    
    /// <summary>
    /// Добавить новую заявку на бронирование
    /// </summary>
    /// <param name="ticket">Заявка на бронирование</param>
    /// <returns>Task, представляющий задачу на добавления новой заявки</returns>
    Task AddTicketAsync(Ticket ticket);
    
    /// <summary>
    /// Обновить существующую заявку на бронирование
    /// </summary>
    /// <param name="ticket">Заявка на бронирование</param>
    /// <returns>Task, представляющий задачу на обновление заявки</returns>
    Task UpdateTicketAsync(Ticket ticket);
    
    /// <summary>
    /// Удалить заявку на бронирование
    /// </summary>
    /// <param name="ticket">Заявка на бронирование</param>
    /// <returns>Task, представляющий задачу на отмену заявки</returns>
    Task RemoveTicketAsync(Ticket ticket);
}