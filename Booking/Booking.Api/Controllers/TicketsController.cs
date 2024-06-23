using System.Security.Claims;
using Booking.Domain.DataObjects;
using Booking.Domain.Entities;
using Booking.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    [Authorize(Roles = "Manager, Admin, Service")]
    public async Task<IEnumerable<Ticket>> GetAll()
    {
        return await _ticketService.GetAll();
    }

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

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<Ticket>> CreateTicket(TicketData data)
    {
        var ticket = await _ticketService.CreateTicket(Guid.Parse(User.FindFirstValue("id")!), data);

        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> SwitchConfirmationStatus(Guid id)
    {
        await _ticketService.SwitchConfirmationStatus(id);

        return NoContent();
    }

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