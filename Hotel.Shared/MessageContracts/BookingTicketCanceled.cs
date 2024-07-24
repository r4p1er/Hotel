namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Событие отмены заявки на бронирование
/// </summary>
public class BookingTicketCanceled
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
    /// Идентификатор бронирубщего пользователя
    /// </summary>
    public Guid UserId { get; set; }
}