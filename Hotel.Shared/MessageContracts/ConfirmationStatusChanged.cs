namespace Hotel.Shared.MessageContracts;

public class ConfirmationStatusChanged
{
    public Guid Id { get; set; }
    
    public Guid RoomId { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool ConfirmationStatus { get; set; }
}