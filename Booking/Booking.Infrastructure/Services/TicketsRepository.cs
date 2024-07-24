using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Services;

public class TicketsRepository : ITicketsRepository
{
    private readonly ApplicationContext _context;

    public TicketsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public IQueryable<Ticket> FindAll()
    {
        return _context.Tickets;
    }

    public async Task<Ticket?> FindByIdAsync(Guid id)
    {
        return await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddTicketAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveTicketAsync(Ticket ticket)
    {
        _context.Tickets.Remove(ticket);

        await _context.SaveChangesAsync();
    }
}