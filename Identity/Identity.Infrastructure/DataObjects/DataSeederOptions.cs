namespace Identity.Infrastructure.DataObjects;

/// <summary>
/// Опции для сервиса DataSeeder
/// </summary>
public class DataSeederOptions
{
    /// <summary>
    /// Пароль для администратора
    /// </summary>
    public string AdminPassword { get; set; }
    
    /// <summary>
    /// Пароль для сервисвной учетной записи
    /// </summary>
    public string ServicePassword { get; set; }
    
    /// <summary>
    /// Перец. Нужен для дополнительной защиты
    /// </summary>
    public string Pepper { get; set; }
}