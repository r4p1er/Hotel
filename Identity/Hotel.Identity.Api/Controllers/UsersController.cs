using System.Security.Claims;
using Hotel.Identity.Domain.Abstractions;
using Hotel.Identity.Domain.Entities;
using Hotel.Identity.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Identity.Api.Controllers;

/// <summary>
/// Web API контроллер пользователей
/// </summary>
/// <param name="userService">Сервис для работы с пользователями</param>
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Залогиниться, получить JWT
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <param name="password">Пароль</param>
    /// <returns>OkObject с JWT в поле Token</returns>
    [HttpGet("login")]
    public async Task<IActionResult> Login(string email, [FromHeader]string password)
    {
        var token = await userService.Login(email, password);

        return Ok(new { Token = token });
    }

    /// <summary>
    /// Филтрованный поиск пользователей
    /// </summary>
    /// <param name="filters">Фильтры</param>
    /// <returns>Коллекция пользователей</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Service")]
    public async Task<IEnumerable<User>> Search([FromQuery]QueryFiltersData filters)
    {
        return await userService.GetAll(filters);
    }

    /// <summary>
    /// Получить пользователя по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователь</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Service")]
    public async Task<User> GetById(Guid id)
    {
        var user = await userService.GetById(id);

        return user;
    }

    /// <summary>
    /// Получить связанного с собой пользователя на основе данных JWT
    /// </summary>
    /// <returns>Пользователь</returns>
    [HttpGet("self")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<User> GetSelf()
    {
        var user = await userService.GetById(Guid.Parse(User.FindFirstValue("id")!));

        return user;
    }

    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>Объект пользователя, обернутый в CreatedAtAction</returns>
    [HttpPost]
    public async Task<ActionResult<User>> RegisterUser(RegisterData data)
    {
        var user = await userService.RegisterUser(data);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Зарегистрировать нового члена персонала
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>Объект пользователя, обернутый в CreatedAtAction</returns>
    [HttpPost("managers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> RegisterManager(RegisterData data)
    {
        var user = await userService.RegisterManager(data);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Изменить данные о себе
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <returns>NoContent</returns>
    [HttpPatch("me")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IActionResult> EditSelf(RegisterData data)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await userService.EditSelf(id, data);

        return NoContent();
    }

    /// <summary>
    /// Изменить пароль у себя
    /// </summary>
    /// <param name="password">Новый пароль</param>
    /// <returns>NoContent</returns>
    [HttpPatch("me/password")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IActionResult> EditSelfPassword([FromHeader]string password)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await userService.EditPassword(id, password);

        return NoContent();
    }

    /// <summary>
    /// Изменить статус блокировки пользователя на противоположный
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>NoContent</returns>
    [HttpPatch("{id}/block")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleUserBlock(Guid id)
    {
        await userService.ToggleUserBlock(id);

        return NoContent();
    }
}