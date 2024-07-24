namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Событие о изменении статуса заявки
/// </summary>
public class ConfirmationStatusChanged
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
    /// Статус заявки на бронирование (подтверждена или нет)
    /// </summary>
    public bool ConfirmationStatus { get; set; }
}