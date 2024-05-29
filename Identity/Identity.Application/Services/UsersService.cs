using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class UsersService
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly IConfiguration _configuration;
    private User? _currentUser;

    public UsersService(IUsersRepository repository, IPasswordHasher hasher, IConfiguration configuration)
    {
        _repository = repository;
        _hasher = hasher;
        _configuration = configuration;
    }

    public async Task SetCurrentUser(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);

        _currentUser = user ?? throw new ArgumentException("User id is invalid");
    }

    private void CheckRight(Right right)
    {
        if (_currentUser == null) throw new InvalidOperationException("Current user is not assigned");
        
        if (RightPermissionMapper.RightToPermission(_currentUser.Role, right) == Permission.NoAccess || _currentUser.IsBlocked)
        {
            throw new PermissionDeniedException();
        }
    }
    
    public async Task<string?> Login(string email, string password)
    {
        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;
        var user = await _repository.FindByEmailAsync(email);
        
        if (user == null || !_hasher.Verify(password + pepper, user.PasswordHash)) return null;

        if (_configuration["Auth:Key"] == null || _configuration["Auth:Expires"] == null)
        {
            throw new ConfigurationMissingException("Auth config is missed");
        }

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Key"]!)),
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: new List<Claim>() { new Claim("userId", user.Id.ToString()) },
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Auth:Expires"]))
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User?> GetById(Guid id)
    {
        CheckRight(Right.ReadUserById);

        return await _repository.FindByIdAsync(id);
    }

    public User GetSelf()
    {
        CheckRight(Right.ReadSelf);

        return _currentUser!;
    }

    private async Task Register(RegisterDTO dto, Role role)
    {
        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PasswordHash = _hasher.HashPassword(dto.Password + pepper),
            Role = role,
            IsBlocked = false
        };

        await _repository.AddUserAsync(user);
    }

    public async Task RegisterUser(RegisterDTO dto)
    {
        await Register(dto, Role.User);
    }

    public async Task RegisterManager(RegisterDTO dto)
    {
        CheckRight(Right.CreateManager);

        await Register(dto, Role.Manager);
    }

    public async Task EditSelf(RegisterDTO dto)
    {
        CheckRight(Right.UpdateSelf);

        _currentUser!.Name = dto.Name;
        _currentUser.Surname = dto.Surname;
        _currentUser.Patronymic = dto.Patronymic;
        _currentUser.PhoneNumber = dto.PhoneNumber;
        _currentUser.Email = dto.Email;

        await _repository.UpdateUserAsync(_currentUser);
    }

    public async Task EditPassword(string password)
    {
        CheckRight(Right.UpdatePassword);

        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;
        _currentUser!.PasswordHash = _hasher.HashPassword(password + pepper);

        await _repository.UpdateUserAsync(_currentUser);
    }

    public async Task ToggleUserBlock(Guid id)
    {
        CheckRight(Right.BlockUser);

        var user = await _repository.FindByIdAsync(id);
        if (user == null) throw new ArgumentException("User id is invalid");
        user.IsBlocked = !user.IsBlocked;

        await _repository.UpdateUserAsync(user);
    }
}