namespace Hotel.Shared.MessageContracts;

public class BookingTicketCreated
{
    public Guid Id { get; set; }
    
    public Guid RoomId { get; set; }
    
    public Guid UserId { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public decimal Price { get; set; }
}