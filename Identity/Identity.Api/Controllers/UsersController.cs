using System.Security.Claims;
using Identity.Domain.DataObjects;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string email, [FromHeader]string password)
    {
        var token = await _userService.Login(email, password);

        return token == null ? NotFound() : Ok(new { Token = token });
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Service")]
    public async Task<IEnumerable<User>> Search([FromQuery]QueryFiltersData filters)
    {
        return await _userService.GetAll(filters);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Service")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _userService.GetById(id);

        return user == null ? NotFound() : user;
    }

    [HttpGet("self")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<ActionResult<User>> GetSelf()
    {
        var user = await _userService.GetById(Guid.Parse(User.FindFirstValue("id")!));

        if (user == null) return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> RegisterUser(RegisterData data)
    {
        var user = await _userService.RegisterUser(data);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPost("managers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> RegisterManager(RegisterData data)
    {
        var user = await _userService.RegisterManager(data);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPatch("me")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IActionResult> EditSelf(RegisterData data)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await _userService.EditSelf(id, data);

        return NoContent();
    }

    [HttpPatch("me/password")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IActionResult> EditSelfPassword([FromHeader]string password)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await _userService.EditPassword(id, password);

        return NoContent();
    }

    [HttpPatch("{id}/block")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleUserBlock(Guid id)
    {
        await _userService.ToggleUserBlock(id);

        return NoContent();
    }
}