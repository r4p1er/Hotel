using Hotel.Shared.Exceptions;
using Managing.Domain.DataObjects;
using Managing.Domain.Entities;
using Managing.Domain.Enums;
using Managing.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Managing.Domain.Services;

public class RoomService
{
    private readonly IRoomsRepository _repository;

    public RoomService(IRoomsRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<Room>> GetAll(QueryFiltersData filters)
    {
        var rooms = _repository.FindAll();

        rooms = string.IsNullOrEmpty(filters.Search)
            ? rooms
            : rooms.Where(x =>
                x.Summary.ToLower().Contains(filters.Search.ToLower()) ||
                x.Description.ToLower().Contains(filters.Search.ToLower()));
        rooms = filters.Offset == null ? rooms : rooms.Skip(filters.Offset.Value);
        rooms = filters.Limit == null ? rooms : rooms.Take(filters.Limit.Value);

        if (!string.IsNullOrWhiteSpace(filters.SortBy) && filters.SortOrder != null)
        {
            rooms = rooms.OrderByName(filters.SortBy, filters.SortOrder.Value);
        }
        else if (!string.IsNullOrWhiteSpace(filters.SortBy))
        {
            rooms = rooms.OrderByName(filters.SortBy);
        }
        else if (filters.SortOrder != null)
        {
            rooms = filters.SortOrder == SortOrder.Asc
                ? rooms.OrderBy(x => x.Id)
                : rooms.OrderByDescending(x => x.Id);
        }

        return await rooms.ToListAsync();
    }

    public async Task<Room> GetById(Guid id)
    {
        var room = await _repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        return room;
    }

    public async Task<Room> CreateRoom(RoomData data)
    {
        var room = new Room()
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            Description = data.Description,
            Price = data.Price
        };

        await _repository.AddRoomAsync(room);

        return room;
    }

    public async Task EditRoom(Guid id, RoomData data)
    {
        var room = await _repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        room.Summary = data.Summary;
        room.Description = data.Description;
        room.Price = data.Price;

        await _repository.UpdateRoomAsync(room);
    }

    public async Task DeleteRoom(Guid id)
    {
        var room = await _repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        await _repository.RemoveRoomAsync(room);
    }
}