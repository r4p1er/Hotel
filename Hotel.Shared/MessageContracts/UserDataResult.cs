namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Результат команды на выборку имени и email определенного пользователя
/// </summary>
public class UserDataResult
{
    /// <summary>
    /// Идентификатор бронирующего пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }
}