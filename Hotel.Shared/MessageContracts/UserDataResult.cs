namespace Hotel.Shared.MessageContracts;

public class UserDataResult
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string Name { get; set; }
}