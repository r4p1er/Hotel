using Booking.Domain.DataObjects;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using FluentValidation;
using Hotel.Shared.Exceptions;
using Hotel.Shared.MessageContracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Booking.Domain.Services;

public class TicketService(ITicketsRepository repository, 
    IValidator<TicketData> validator,
    IPublishEndpoint publishEndpoint) : ITicketService
{
    public async Task<IEnumerable<Ticket>> GetAll()
    {
        return await repository.FindAll().ToListAsync();
    }

    public async Task<Ticket> GetById(Guid id)
    {
        var ticket = await repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        return ticket;
    }

    public async Task<Ticket> CreateTicket(Guid userId, TicketData data)
    {
        await validator.ValidateAndThrowAsync(data);

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoomId = data.RoomId,
            From = data.From,
            To = data.To,
            Price = data.Price,
            ConfirmationStatus = false
        };

        await repository.AddTicketAsync(ticket);

        await publishEndpoint.Publish<BookingTicketCreated>(new BookingTicketCreated
        {
            Id = ticket.Id,
            RoomId = ticket.RoomId,
            UserId = ticket.UserId,
            From = ticket.From,
            To = ticket.To,
            Price = ticket.Price
        });

        return ticket;
    }

    public async Task SwitchConfirmationStatus(Guid id)
    {
        var ticket = await repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        ticket.ConfirmationStatus = !ticket.ConfirmationStatus;
        
        await repository.UpdateTicketAsync(ticket);

        await publishEndpoint.Publish<ConfirmationStatusChanged>(new ConfirmationStatusChanged
        {
            Id = ticket.Id,
            RoomId = ticket.RoomId,
            UserId = ticket.UserId,
            ConfirmationStatus = ticket.ConfirmationStatus
        });
    }

    public async Task CancelTicket(Guid id)
    {
        var ticket = await repository.FindByIdAsync(id);

        if (ticket == null) throw new NotFoundException("Ticket not found");

        await repository.RemoveTicketAsync(ticket);

        await publishEndpoint.Publish<BookingTicketCanceled>(new BookingTicketCanceled
        {
            Id = ticket.Id,
            RoomId = ticket.RoomId,
            UserId = ticket.UserId
        });
    }
}