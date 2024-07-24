namespace Hotel.Shared.Models;

public class BookingTicket
{
    public Guid RoomId { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
}