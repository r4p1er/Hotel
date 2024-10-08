using Hotel.Booking.Domain.Abstractions;
using Hotel.Booking.Domain.Entities;
using Hotel.Booking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Booking.Infrastructure.Services;

/// <inheritdoc cref="ITicketsRepository"/>
public class TicketsRepository : ITicketsRepository
{
    /// <inheritdoc cref="ApplicationContext"/>
    private readonly ApplicationContext _context;

    /// <summary/>
    public TicketsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc cref="ITicketsRepository.FindAll"/>
    public IQueryable<Ticket> FindAll()
    {
        return _context.Tickets;
    }

    /// <inheritdoc cref="ITicketsRepository.FindByIdAsync"/>
    public async Task<Ticket?> FindByIdAsync(Guid id)
    {
        return await _context.Tickets.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc cref="ITicketsRepository.AddTicketAsync"/>
    public async Task AddTicketAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc cref="ITicketsRepository.UpdateTicketAsync"/>
    public async Task UpdateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc cref="ITicketsRepository.RemoveTicketAsync"/>
    public async Task RemoveTicketAsync(Ticket ticket)
    {
        _context.Tickets.Remove(ticket);

        await _context.SaveChangesAsync();
    }
}