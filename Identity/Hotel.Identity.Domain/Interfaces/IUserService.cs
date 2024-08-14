using Hotel.Identity.Domain.DataObjects;
using Hotel.Identity.Domain.Entities;

namespace Hotel.Identity.Domain.Interfaces;

/// <summary>
/// Сервис для рбаоты с пользователями
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Залогиниться, получить JWT
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    /// <returns>строка JWT</returns>
    Task<string> Login(string email, string password);
    
    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    /// <param name="filters">Фильтры запроса</param>
    /// <returns>Коллекция пользователей</returns>
    Task<IEnumerable<User>> GetAll(QueryFiltersData filters);
    
    /// <summary>
    /// Получить пользователя по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователь</returns>
    Task<User> GetById(Guid id);
    
    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>Пользователь</returns>
    Task<User> RegisterUser(RegisterData data);
    
    /// <summary>
    /// Зарегистрировать нового сотрудника
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>Пользователь</returns>
    Task<User> RegisterManager(RegisterData data);
    
    /// <summary>
    /// Изменить данные пользователя по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>Task, представляющий задачу изменения данных о пользователе</returns>
    Task EditSelf(Guid id, RegisterData data);
    
    /// <summary>
    /// Изменить пароль пользователя по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="password">Новый пароль</param>
    /// <returns>Task, представляющий задачу изменения пароля пользователя</returns>
    Task EditPassword(Guid id, string password);
    
    /// <summary>
    /// Изменить статус блокировки пользователя на противоположный
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Task, представляющий задачу изменения статуса блокировки пользователя</returns>
    Task ToggleUserBlock(Guid id);
}