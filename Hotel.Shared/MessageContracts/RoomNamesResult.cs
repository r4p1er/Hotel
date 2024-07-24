using Hotel.Shared.Models;

namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Результат команды SelectRoomNames
/// </summary>
public class RoomNamesResult
{
    /// <summary>
    /// Лист с объектами RoomData
    /// </summary>
    public List<RoomData> Rooms { get; set; }
}