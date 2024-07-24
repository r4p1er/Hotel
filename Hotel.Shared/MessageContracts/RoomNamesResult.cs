using Hotel.Shared.Models;

namespace Hotel.Shared.MessageContracts;

public class RoomNamesResult
{
    public List<RoomData> Rooms { get; set; }
}