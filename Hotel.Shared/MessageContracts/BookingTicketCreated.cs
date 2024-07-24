namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Событие создания заявки на бронирование
/// </summary>
public class BookingTicketCreated
{
    /// <summary>
    /// Идентификатор заявки на бронирование
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid RoomId { get; set; }
    
    /// <summary>
    /// Идентификатор бронирующего пользователя
    /// </summary>
    public Guid UserId { get; set; }
    
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