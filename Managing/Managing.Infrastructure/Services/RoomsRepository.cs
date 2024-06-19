using Managing.Domain.Entities;
using Managing.Domain.Interfaces;
using Managing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Managing.Infrastructure.Services;

public class RoomsRepository : IRoomsRepository
{
    private readonly ApplicationContext _context;

    public RoomsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public IQueryable<Room> FindAll()
    {
        return _context.Rooms;
    }

    public async Task<Room?> FindByIdAsync(Guid id)
    {
        return await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddRoomAsync(Room room)
    {
        await _context.AddAsync(room);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoomAsync(Room room)
    {
        _context.Rooms.Update(room);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoomAsync(Room room)
    {
        _context.Rooms.Remove(room);

        await _context.SaveChangesAsync();
    }
}