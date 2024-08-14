namespace Hotel.Booking.Domain.DataObjects;

/// <summary>
/// Данные для создания заявки на бронирование
/// </summary>
public class TicketData
{
    /// <summary>
    /// Идентификатор комнаты
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
}