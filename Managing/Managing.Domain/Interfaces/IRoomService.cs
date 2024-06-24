using Managing.Domain.DataObjects;
using Managing.Domain.Entities;

namespace Managing.Domain.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAll(QueryFiltersData filters);
    Task<Room> GetById(Guid id);
    Task<Room> CreateRoom(RoomData data);
    Task EditRoom(Guid id, RoomData data);
    Task DeleteRoom(Guid id);
}