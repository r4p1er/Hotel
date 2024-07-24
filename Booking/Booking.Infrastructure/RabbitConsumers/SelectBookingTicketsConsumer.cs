using Booking.Domain.Interfaces;
using Hotel.Shared.MessageContracts;
using Hotel.Shared.Models;
using MassTransit;

namespace Booking.Infrastructure.RabbitConsumers;

public class SelectBookingTicketsConsumer(ITicketService ticketService) : IConsumer<SelectBookingTickets>
{
    public async Task Consume(ConsumeContext<SelectBookingTickets> context)
    {
        var fromDate = context.Message.From;
        var toDate = context.Message.To;
        var tickets = await ticketService.GetAll();
        tickets = tickets.Where(x =>
                x.From.Date >= fromDate.Date && x.To.Date <= toDate.Date && x.ConfirmationStatus)
            .ToList();
        var result = new List<BookingTicket>();

        foreach (var ticket in tickets)
        {
            result.Add(new BookingTicket
            {
                RoomId = ticket.RoomId,
                From = ticket.From,
                To = ticket.To
            });
        }

        await context.RespondAsync<BookingTicketsResult>(new BookingTicketsResult { BookingTickets = result });
    }
}