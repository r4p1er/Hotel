namespace Reporting.Domain.DataObjects;

public class BookingData
{
    public Guid RoomId { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
}