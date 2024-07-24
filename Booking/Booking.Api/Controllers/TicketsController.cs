using System.Security.Claims;
using Booking.Domain.DataObjects;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers;

/// <summary>
/// Web API контроллер заявок на бронирование
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    /// <summary>
    /// <inheritdoc cref="ITicketService"/>
    /// </summary>
    private readonly ITicketService _ticketService;

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
    public async Task<IEnumerable<Ticket>> GetAll()
    {
        return await _ticketService.GetAll();
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
            return Forbid();
        }

        return ticket;
    }

    /// <summary>
    /// Создать заявку на бронирование
    /// </summary>
    /// <param name="data"><inheritdoc cref="TicketData"/></param>
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
    /// <returns><inheritdoc cref="IActionResult"/></returns>
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
    /// <returns><inheritdoc cref="IActionResult"/></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CancelTicket(Guid id)
    {
        var ticket = await _ticketService.GetById(id);

        if (Guid.Parse(User.FindFirstValue("id")!) != ticket.UserId)
        {
            return Forbid();
        }        
        
        await _ticketService.CancelTicket(id);
        
        return NoContent();
    }
}