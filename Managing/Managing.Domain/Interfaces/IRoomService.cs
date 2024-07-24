using Managing.Domain.DataObjects;
using Managing.Domain.Entities;

namespace Managing.Domain.Interfaces;

/// <summary>
/// Сервис для работы с номерами отеля
/// </summary>
public interface IRoomService
{
    /// <summary>
    /// Получить все номера отеля
    /// </summary>
    /// <param name="filters">Фильтры запроса</param>
    /// <returns>Коллекция номеров отеля</returns>
    Task<IEnumerable<Room>> GetAll(QueryFiltersData filters);
    
    /// <summary>
    /// Получить номер отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <returns>Номер отеля</returns>
    Task<Room> GetById(Guid id);
    
    /// <summary>
    /// Создать новый номер отеля
    /// </summary>
    /// <param name="data">Данные нового номера отеля</param>
    /// <returns>Созданный номер отеля</returns>
    Task<Room> CreateRoom(RoomData data);
    
    /// <summary>
    /// Изменить данные существующего номера отеля
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <param name="data">Данные номера отеля</param>
    /// <returns>Task, прдеставляющий текующую задачу</returns>
    Task EditRoom(Guid id, RoomData data);
    
    /// <summary>
    /// Удалить номер отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <returns>Task, прдеставляющий текующую задачу</returns>
    Task DeleteRoom(Guid id);
}