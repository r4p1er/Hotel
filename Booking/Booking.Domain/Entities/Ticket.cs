namespace Booking.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid RoomId { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public decimal Price { get; set; }

    public bool ConfirmationStatus { get; set; } = false;
}