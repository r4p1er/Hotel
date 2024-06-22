using Managing.Domain.Entities;

namespace Managing.Domain.Interfaces;

public interface IRoomsRepository
{
    IQueryable<Room> FindAll();
    Task<Room?> FindByIdAsync(Guid id);
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task RemoveRoomAsync(Room room);
}