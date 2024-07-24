namespace Booking.Domain.Entities;

/// <summary>
/// Заявка на бронирование
/// </summary>
public class Ticket
{
    /// <summary>
    /// Идентификатор заявки на бронирование
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Иденитификатор пользователи, бронирующего номер
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор бронируемого номера
    /// </summary>
    public Guid RoomId { get; set; }
    
    
    /// <summary>
    /// С какого числа и времени бронирование
    /// </summary>
    public DateTime From { get; set; }
    
    /// <summary>
    /// По какое число и время бронирование
    /// </summary>
    public DateTime To { get; set; }
    
    /// <summary>
    /// Цена за номер
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Статус заявки на бронирование (подтверждена или нет)
    /// </summary>
    public bool ConfirmationStatus { get; set; } = false;
}