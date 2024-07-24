namespace Hotel.Shared.Models;

/// <summary>
/// Данные о заявках, запршиваемые в команде SelectBookingTickets
/// </summary>
public class BookingTicket
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
}