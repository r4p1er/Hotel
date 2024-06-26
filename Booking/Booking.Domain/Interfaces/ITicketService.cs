using Booking.Domain.DataObjects;
using Booking.Domain.Entities;

namespace Booking.Domain.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAll();
    Task<Ticket> GetById(Guid id);
    Task<Ticket> CreateTicket(Guid userId, TicketData data);
    Task SwitchConfirmationStatus(Guid id);
    Task CancelTicket(Guid id);
}