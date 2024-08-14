namespace Hotel.Identity.Infrastructure.Interfaces;

/// <summary>
/// Сервис для заполнения БД начальными данными
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    /// Заполнить БД начальными данными
    /// </summary>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task SeedAsync();
}