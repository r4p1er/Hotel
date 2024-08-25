using Hotel.Managing.Domain.Abstractions;
using Hotel.Managing.Domain.Entities;
using Hotel.Managing.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Managing.Api.Controllers;

/// <summary>
/// Web API контроллер номеров отеля
/// </summary>
/// <param name="roomService">Сервис для работы с номерами отеля</param>
[ApiController]
[Route("api/[controller]")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    /// <summary>
    /// Поиск номеров отеля с фильтрами
    /// </summary>
    /// <param name="filters">Фильтры поиска</param>
    /// <returns>Коллекция номеров отеля</returns>
    [HttpGet]
    public async Task<IEnumerable<Room>> Search([FromQuery] QueryFiltersData filters)
    {
        return await roomService.GetAll(filters);
    }

    /// <summary>
    /// Получить номер отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номеря отеля</param>
    /// <returns>Номер отеля</returns>
    [HttpGet("{id}")]
    public async Task<Room> GetById(Guid id)
    {
        return await roomService.GetById(id);
    }

    /// <summary>
    /// Создать новый номер отеля
    /// </summary>
    /// <param name="data">Данные нового номера отеля</param>
    /// <returns>Номер отеля, обернутый в CreatedAtAction</returns>
    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<ActionResult<Room>> Create(RoomData data)
    {
        var room = await roomService.CreateRoom(data);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    /// <summary>
    /// Изменить данные номера отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <param name="data">Данные номера отеля</param>
    /// <returns>NoContent</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> Edit(Guid id, RoomData data)
    {
        await roomService.EditRoom(id, data);

        return NoContent();
    }

    /// <summary>
    /// Удалить номер отеля по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор номера отеля</param>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await roomService.DeleteRoom(id);

        return NoContent();
    }
}