using Hotel.Shared.Models;

namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Результат команды SelectBookingTickets
/// </summary>
public class BookingTicketsResult
{
    /// <summary>
    /// Лист объектор BookingTicket
    /// </summary>
    public List<BookingTicket> BookingTickets { get; set; }
}