namespace Hotel.Shared.MessageContracts;

public class SelectBookingTickets
{
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
}