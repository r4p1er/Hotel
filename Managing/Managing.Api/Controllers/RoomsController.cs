using Managing.Domain.DataObjects;
using Managing.Domain.Entities;
using Managing.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Managing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IEnumerable<Room>> Search([FromQuery] QueryFiltersData filters)
    {
        return await _roomService.GetAll(filters);
    }

    [HttpGet("{id}")]
    public async Task<Room> GetById(Guid id)
    {
        return await _roomService.GetById(id);
    }

    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult<Room>> Create(RoomData data)
    {
        var room = await _roomService.CreateRoom(data);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> Edit(Guid id, RoomData data)
    {
        await _roomService.EditRoom(id, data);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _roomService.DeleteRoom(id);

        return NoContent();
    }
}