namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Команда на выборку подтвержденных заявок на бронирование за определенный период
/// </summary>
public class SelectBookingTickets
{
    /// <summary>
    /// С какого числа и времени бронирование
    /// </summary>
    public DateTime From { get; set; }
    
    /// <summary>
    /// По какое число и время бронирование
    /// </summary>
    public DateTime To { get; set; }
}