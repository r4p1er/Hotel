namespace Hotel.Shared.MessageContracts;

public class BookingTicketCanceled
{
    public Guid Id { get; set; }
    
    public Guid RoomId { get; set; }
    
    public Guid UserId { get; set; }
}