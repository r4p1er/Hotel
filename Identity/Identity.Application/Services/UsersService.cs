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

    public async Task<IEnumerable<User>> GetAll(QueryFiltersData filters)
    {
        var users = await _repository.FindAllAsync();
        
        users = string.IsNullOrEmpty(filters.Search)
            ? users
            : users.Where(x => x.Surname.ToLower().Contains(filters.Search.ToLower()) ||
                               x.Name.ToLower().Contains(filters.Search.ToLower()) ||
                               (x.Patronymic != null && x.Patronymic.ToLower().Contains(filters.Search.ToLower())) ||
                               x.Email.ToLower().Contains(filters.Search.ToLower()) ||
                               x.PhoneNumber.ToLower().Contains(filters.Search.ToLower()));
        users = filters.Offset == null ? users : users.Skip(filters.Offset.Value);
        users = filters.Limit == null ? users : users.Take(filters.Limit.Value);

        if (!string.IsNullOrWhiteSpace(filters.SortBy) && filters.SortOrder != null)
        {
            var prop = typeof(User).GetProperty(filters.SortBy);

            if (prop == null) throw new ArgumentException("Name of prop to order is invalid");

            users = filters.SortOrder == SortOrder.Asc
                ? users.OrderBy(x => prop.GetValue(x))
                : users.OrderByDescending(x => prop.GetValue(x));
        }
        else if (!string.IsNullOrWhiteSpace(filters.SortBy))
        {
            var prop = typeof(User).GetProperty(filters.SortBy);

            if (prop == null) throw new ArgumentException("Name of prop to order is invalid");

            users = users.OrderBy(x => prop.GetValue(x));
        }
        else if (filters.SortOrder != null)
        {
            users = filters.SortOrder == SortOrder.Asc ? users.Order() : users.OrderDescending();
        }

        return users.ToList();
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _repository.FindByIdAsync(id);
    }

    private async Task Register(RegisterData data, Role role)
    {
        if (await _repository.FindByEmailAsync(data.Email) != null)
            throw new ArgumentException("The email is already used");
        
        string pepper = _configuration["Auth:Pepper"] ?? string.Empty;

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Surname = data.Surname,
            Patronymic = data.Patronymic,
            PhoneNumber = data.PhoneNumber,
            Email = data.Email,
            PasswordHash = _hasher.HashPassword(data.Password + pepper),
            Role = role,
            IsBlocked = false
        };

        await _repository.AddUserAsync(user);
    }

    public async Task RegisterUser(RegisterData data)
    {
        await Register(data, Role.User);
    }

    public async Task RegisterManager(RegisterData data)
    {
        await Register(data, Role.Manager);
    }

    public async Task EditSelf(Guid id, RegisterData data)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new ArgumentException("User id is invalid");
        
        user.Name = data.Name;
        user.Surname = data.Surname;
        user.Patronymic = data.Patronymic;
        user.PhoneNumber = data.PhoneNumber;
        user.Email = data.Email;

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