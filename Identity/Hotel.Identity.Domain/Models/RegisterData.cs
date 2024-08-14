namespace Hotel.Identity.Domain.Models;

/// <summary>
/// Данные для регистрации нового пользователя
/// </summary>
public class RegisterData
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Телефонный номер
    /// </summary>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
}