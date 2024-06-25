using Booking.Domain.DataObjects;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using FluentValidation;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Booking.Domain.Services;

public class TicketService : ITicketService
{
    private readonly ITicketsRepository _repository;
    private readonly IValidator<TicketData> _validator;

    public TicketService(ITicketsRepository repository, IValidator<TicketData> validator)
    {
        _repository = repository;
        _validator = validator;
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

    public async Task<Ticket> CreateTicket(Guid userId, TicketData data)
    {
        await _validator.ValidateAndThrowAsync(data);

        var ticket = new Ticket()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
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