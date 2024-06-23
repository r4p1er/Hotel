namespace Booking.Domain.DataObjects;

public class TicketData
{
    public Guid UserId { get; set; }
    
    public Guid RoomId { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public decimal Price { get; set; }
}