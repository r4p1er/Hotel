using Hotel.Shared.Models;

namespace Hotel.Shared.MessageContracts;

public class BookingTicketsResult
{
    public List<BookingTicket> BookingTickets { get; set; }
}