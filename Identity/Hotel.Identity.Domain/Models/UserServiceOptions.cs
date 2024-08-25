namespace Hotel.Identity.Domain.Models;

/// <summary>
/// Опции для сервиса UserService
/// </summary>
public class UserServiceOptions
{
    /// <summary>
    /// Перец. Используется для допольнительной защиты
    /// </summary>
    public string Pepper { get; set; }

    /// <summary>
    /// Ключ для шифрования JWT
    /// </summary>
    public string Key { get; set; }
    
    /// <summary>
    /// Количество минут, после истечения которых JWT устаревает
    /// </summary>
    public int Expires { get; set; }
}