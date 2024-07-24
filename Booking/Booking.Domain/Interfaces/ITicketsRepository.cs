using Booking.Domain.Entities;

namespace Booking.Domain.Interfaces;

public interface ITicketsRepository
{
    IQueryable<Ticket> FindAll();
    Task<Ticket?> FindByIdAsync(Guid id);
    Task AddTicketAsync(Ticket ticket);
    Task UpdateTicketAsync(Ticket ticket);
    Task RemoveTicketAsync(Ticket ticket);
}