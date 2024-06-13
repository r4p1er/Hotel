using System.Security.Claims;
using Identity.Domain.DataObjects;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    public async Task<IResult> Login(string email, [FromHeader]string password)
    {
        var token = await _userService.Login(email, password);

        return token == null ? Results.NotFound() : Results.Ok(new { Token = token });
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Service")]
    public IResult Search([FromQuery]QueryFiltersData filters)
    {
        return Results.Json(_userService.GetAll(filters));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Service")]
    public async Task<IResult> GetById(Guid id)
    {
        var user = await _userService.GetById(id);

        return user == null ? Results.NotFound() : Results.Json(user);
    }

    [HttpGet("self")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IResult> GetSelf()
    {
        return Results.Json(await _userService.GetById(Guid.Parse(User.FindFirstValue("id")!)));
    }

    [HttpPost]
    public async Task<IResult> RegisterUser(RegisterData data)
    {
        await _userService.RegisterUser(data);

        return Results.Created();
    }

    [HttpPost("managers")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> RegisterManager(RegisterData data)
    {
        await _userService.RegisterManager(data);

        return Results.Created();
    }

    [HttpPut("self")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IResult> EditSelf(RegisterData data)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await _userService.EditSelf(id, data);

        return Results.NoContent();
    }

    [HttpPut("self/password")]
    [Authorize(Roles = "User, Manager, Admin")]
    public async Task<IResult> EditSelfPassword([FromHeader]string password)
    {
        var id = Guid.Parse(User.FindFirstValue("id")!);
        await _userService.EditPassword(id, password);

        return Results.NoContent();
    }

    [HttpPut("block/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> ToggleUserBlock(Guid id)
    {
        await _userService.ToggleUserBlock(id);

        return Results.NoContent();
    }
}