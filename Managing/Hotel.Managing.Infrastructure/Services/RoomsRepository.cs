using Hotel.Managing.Domain.Abstractions;
using Hotel.Managing.Domain.Entities;
using Hotel.Managing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Managing.Infrastructure.Services;

/// <summary>
/// Репозиторий с номерами отеля. Реализация IRoomsRepository
/// </summary>
/// <param name="context">Контекст БД</param>
public class RoomsRepository(ApplicationContext context) : IRoomsRepository
{
    /// <inheritdoc cref="IRoomsRepository.FindAll"/>
    public IQueryable<Room> FindAll()
    {
        return context.Rooms;
    }

    /// <inheritdoc cref="IRoomsRepository.FindByIdAsync"/>
    public async Task<Room?> FindByIdAsync(Guid id)
    {
        return await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc cref="IRoomsRepository.AddRoomAsync"/>
    public async Task AddRoomAsync(Room room)
    {
        await context.AddAsync(room);

        await context.SaveChangesAsync();
    }

    /// <inheritdoc cref="IRoomsRepository.UpdateRoomAsync"/>
    public async Task UpdateRoomAsync(Room room)
    {
        context.Rooms.Update(room);

        await context.SaveChangesAsync();
    }

    /// <inheritdoc cref="IRoomsRepository.RemoveRoomAsync"/>
    public async Task RemoveRoomAsync(Room room)
    {
        context.Rooms.Remove(room);

        await context.SaveChangesAsync();
    }
}