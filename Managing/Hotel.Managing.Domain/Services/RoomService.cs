using FluentValidation;
using Hotel.Managing.Domain.Abstractions;
using Hotel.Managing.Domain.DataObjects;
using Hotel.Managing.Domain.Entities;
using Hotel.Managing.Domain.Enums;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Managing.Domain.Services;

/// <summary>
/// Сервис для работы с номерами отеля. Реализация IRoomService
/// </summary>
/// <param name="repository">Репозиторий номеров отеля</param>
/// <param name="validator">Валидатор данных нового номера отеля</param>
public class RoomService(IRoomsRepository repository, IValidator<RoomData> validator)
    : IRoomService
{
    /// <inheritdoc cref="IRoomService.GetAll"/>
    public async Task<IEnumerable<Room>> GetAll(QueryFiltersData filters)
    {
        var rooms = repository.FindAll();

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

    /// <inheritdoc cref="IRoomService.GetById"/>
    public async Task<Room> GetById(Guid id)
    {
        var room = await repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        return room;
    }

    /// <inheritdoc cref="IRoomService.CreateRoom"/>
    public async Task<Room> CreateRoom(RoomData data)
    {
        await validator.ValidateAndThrowAsync(data);
        
        var room = new Room()
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            Description = data.Description,
            Price = data.Price
        };

        await repository.AddRoomAsync(room);

        return room;
    }

    /// <inheritdoc cref="IRoomService.EditRoom"/>
    public async Task EditRoom(Guid id, RoomData data)
    {
        await validator.ValidateAndThrowAsync(data);
        
        var room = await repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        room.Summary = data.Summary;
        room.Description = data.Description;
        room.Price = data.Price;

        await repository.UpdateRoomAsync(room);
    }

    /// <inheritdoc cref="IRoomService.DeleteRoom"/>
    public async Task DeleteRoom(Guid id)
    {
        var room = await repository.FindByIdAsync(id);

        if (room == null) throw new NotFoundException("Room not found");

        await repository.RemoveRoomAsync(room);
    }
}