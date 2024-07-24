namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Команда на выборку имени и email определенного пользователя
/// </summary>
public class SelectUserData
{
    /// <summary>
    /// Идентификатор бронирующего пользователя
    /// </summary>
    public Guid Id { get; set; }
}