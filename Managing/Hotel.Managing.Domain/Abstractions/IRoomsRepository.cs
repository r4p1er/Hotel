using Hotel.Managing.Domain.Entities;

namespace Hotel.Managing.Domain.Abstractions;

/// <summary>
/// Репозиторий с номерами отеля
/// </summary>
public interface IRoomsRepository
{
    /// <summary>
    /// Получить все номера отеля
    /// </summary>
    /// <returns>IQueryable с номерами отеля</returns>
    IQueryable<Room> FindAll();
    
    /// <summary>
    /// Получить номер отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <returns>Номер отеля, если не найден - null</returns>
    Task<Room?> FindByIdAsync(Guid id);
    
    /// <summary>
    /// Добавить новый номер отеля
    /// </summary>
    /// <param name="room">Номер отеля</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task AddRoomAsync(Room room);
    
    /// <summary>
    /// Обновить данные существующего номера отеля
    /// </summary>
    /// <param name="room">Номер отеля</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task UpdateRoomAsync(Room room);
    
    /// <summary>
    /// Удалить номер отеля
    /// </summary>
    /// <param name="room">Номер отеля</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task RemoveRoomAsync(Room room);
}