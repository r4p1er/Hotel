using Hotel.Identity.Domain.Entities;

namespace Hotel.Identity.Domain.Abstractions;

/// <summary>
/// Репозиторий с пользователями
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Получить пользователя по его электронной почте
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    /// <returns>Пользователь. Если не найден - null</returns>
    Task<User?> FindByEmailAsync(string email);
    
    /// <summary>
    /// Получить пользователя по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователь. Если не найден - null</returns>
    Task<User?> FindByIdAsync(Guid id);
    
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <returns>IQueryable с пользователями</returns>
    IQueryable<User> FindAll();
    
    /// <summary>
    /// Добавить нового пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>Task, представляющий задачу добавления нового пользователя</returns>
    Task AddUserAsync(User user);
    
    /// <summary>
    /// Обность данные существующего пользователя
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <returns>Task, представляющий задачу обновления данных пользователя</returns>
    Task UpdateUserAsync(User user);
}