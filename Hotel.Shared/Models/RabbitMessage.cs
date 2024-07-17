namespace Hotel.Shared.Models;

public class RabbitMessage
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Receiver { get; set; }
    
    public string ResponseTarget { get; set; }
    
    public string Data { get; set; }
}