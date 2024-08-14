using System.Security.Claims;
using Hotel.Booking.Domain.Abstractions;
using Hotel.Booking.Domain.Entities;
using Hotel.Booking.Domain.Models;
using Hotel.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Booking.Api.Controllers;

/// <summary>
/// Web API контроллер заявок на бронирование
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    /// <inheritdoc cref="ITicketService"/>
    private readonly ITicketService _ticketService;

    /// <summary/>
    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// Получить все заявки на бронирование
    /// </summary>
    /// <returns>Коллекция заявок на бронирование</returns>
    [HttpGet]
    [Authorize(Roles = "Manager, Admin, Service")]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetAll()
    {
        return (await _ticketService.GetAll()).ToList();
    }

    /// <summary>
    /// Получить заявку на бронирование по ее идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>Заявка на бронирование. Если запрещено - Forbid</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Ticket>> GetById(Guid id)
    {
        var ticket = await _ticketService.GetById(id);

        if (Guid.Parse(User.FindFirstValue("id")!) != ticket.UserId && User.IsInRole("User"))
        {
            throw new ForbidException("The user doesn't have an access to the ticket");
        }

        return ticket;
    }

    /// <summary>
    /// Создать заявку на бронирование
    /// </summary>
    /// <param name="data">Данные для создания новой заявки</param>
    /// <returns>Заявка на бронирование, обернутая в ActionResult</returns>
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<Ticket>> CreateTicket(TicketData data)
    {
        var ticket = await _ticketService.CreateTicket(Guid.Parse(User.FindFirstValue("id")!), data);

        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    /// <summary>
    /// Изменить статус заявки на бронирование на противоположный
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>NoContent</returns>
    [HttpPatch("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> SwitchConfirmationStatus(Guid id)
    {
        await _ticketService.SwitchConfirmationStatus(id);

        return NoContent();
    }

    /// <summary>
    /// Отменить (удалить) заявку на бронирование
    /// </summary>
    /// <param name="id">Идентификатор заявки на бронирование</param>
    /// <returns>NoContent, если запрещено - Forbid</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CancelTicket(Guid id)
    {
        var ticket = await _ticketService.GetById(id);

        if (Guid.Parse(User.FindFirstValue("id")!) != ticket.UserId)
        {
            throw new ForbidException("The user doesn't have an access to the ticket");
        }        
        
        await _ticketService.CancelTicket(id);
        
        return NoContent();
    }
}