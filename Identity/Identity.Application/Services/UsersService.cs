using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.DTOs;
using Identity.Application.Enums;
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

    public UsersService(IUsersRepository repository, IPasswordHasher hasher, IConfiguration configuration)
    {
        _repository = repository;
        _hasher = hasher;
        _configuration = configuration;
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

    public async Task<IEnumerable<User>> GetAll(QueryFiltersDTO filters)
    {
        var users = await _repository.FindAllAsync();
        
        users = string.IsNullOrEmpty(filters.Search)
            ? users
            : users.Where(x => x.Surname.ToLower().Contains(filters.Search.ToLower()) ||
                               x.Name.ToLower().Contains(filters.Search.ToLower()) ||
                               (x.Patronymic?.ToLower().Contains(filters.Search.ToLower()) ?? false) ||
                               x.Email.ToLower().Contains(filters.Search.ToLower()) ||
                               x.PhoneNumber.ToLower().Contains(filters.Search.ToLower())).ToList();
        users = filters.Offset == null ? users : users.Skip(filters.Offset.Value).ToList();
        users = filters.Limit == null ? users : users.Take(filters.Limit.Value).ToList();

        if (!string.IsNullOrWhiteSpace(filters.SortBy) && filters.SortOrder != null)
        {
            var prop = typeof(User).GetProperty(filters.SortBy);

            if (prop == null) throw new ArgumentException("Name of prop to order is invalid");

            users = filters.SortOrder == SortOrder.Ascending
                ? users.OrderBy(x => prop.GetValue(x)).ToList()
                : users.OrderByDescending(x => prop.GetValue(x)).ToList();
        }
        else if (!string.IsNullOrWhiteSpace(filters.SortBy))
        {
            var prop = typeof(User).GetProperty(filters.SortBy);

            if (prop == null) throw new ArgumentException("Name of prop to order is invalid");

            users = users.OrderBy(x => prop.GetValue(x)).ToList();
        }
        else if (filters.SortOrder != null)
        {
            users = filters.SortOrder == SortOrder.Ascending ? users.Order().ToList() : users.OrderDescending().ToList();
        }

        return users;
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _repository.FindByIdAsync(id);
    }

    private async Task Register(RegisterDTO dto, Role role)
    {
        if (await _repository.FindByEmailAsync(dto.Email) != null)
            throw new ArgumentException("The email is already used");
        
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
        await Register(dto, Role.Manager);
    }

    public async Task EditSelf(Guid id, RegisterDTO dto)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new ArgumentException("User id is invalid");
        
        user.Name = dto.Name;
        user.Surname = dto.Surname;
        user.Patronymic = dto.Patronymic;
        user.PhoneNumber = dto.PhoneNumber;
        user.Email = dto.Email;

        await _repository.UpdateUserAsync(user);
    }

    public async Task EditPassword(Guid id, string password)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new ArgumentException("User id is invalid");
        
        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;
        user.PasswordHash = _hasher.HashPassword(password + pepper);

        await _repository.UpdateUserAsync(user);
    }

    public async Task ToggleUserBlock(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) throw new ArgumentException("User id is invalid");
        user.IsBlocked = !user.IsBlocked;

        await _repository.UpdateUserAsync(user);
    }
}