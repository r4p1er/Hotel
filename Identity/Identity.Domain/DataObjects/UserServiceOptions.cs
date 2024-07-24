namespace Identity.Domain.DataObjects;

public class UserServiceOptions
{
    public string Pepper { get; set; }

    public string Key { get; set; }
    
    public int Expires { get; set; }
}