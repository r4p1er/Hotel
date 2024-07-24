namespace Hotel.Shared.Models;

/// <summary>
/// Данные о комнате, запрашиваемые в команде SelectRoomNames
/// </summary>
public class RoomData
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название комнаты
    /// </summary>
    public string Name { get; set; }
}