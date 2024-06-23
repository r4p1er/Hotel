using Booking.Domain.DataObjects;
using Booking.Domain.Entities;
using Booking.Domain.Exceptions;
using Booking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Domain.Services;

public class TicketService
{
    private readonly ITicketsRepository _repository;

    public TicketService(ITicketsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Ticket>> GetAll()
    {
        return await _repository.FindAll().ToListAsync();
    }

    public async Task<Ticket> GetById(Guid id)
    {
        var ticket = await _repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        return ticket;
    }

    public async Task<Ticket> CreateTicket(TicketData data)
    {
        if (data.To <= data.From) throw new BadRequestException("Invalid dates");

        var ticket = new Ticket()
        {
            Id = Guid.NewGuid(),
            UserId = data.UserId,
            RoomId = data.RoomId,
            From = data.From,
            To = data.To,
            Price = data.Price,
            ConfirmationStatus = false
        };

        await _repository.AddTicketAsync(ticket);

        return ticket;
    }

    public async Task SwitchConfirmationStatus(Guid id)
    {
        var ticket = await _repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        ticket.ConfirmationStatus = !ticket.ConfirmationStatus;
        
        await _repository.UpdateTicketAsync(ticket);
    }

    public async Task CancelTicket(Guid id)
    {
        var ticket = await _repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        await _repository.RemoveTicketAsync(ticket);
    }
}