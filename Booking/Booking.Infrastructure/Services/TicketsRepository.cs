using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Services;

/// <summary>
/// Репозиторий с заявками на бронирование. Реализация интерфейса ITicketsRepository
/// </summary>
public class TicketsRepository : ITicketsRepository
{
    /// <inheritdoc cref="ApplicationContext"/>
    private readonly ApplicationContext _context;

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