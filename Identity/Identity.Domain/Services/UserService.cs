using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hotel.Shared.Exceptions;
using Identity.Domain.DataObjects;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Identity.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Domain.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _repository;
    private readonly UserServiceOptions _options;

    public UserService(IUsersRepository repository, UserServiceOptions options)
    {
        _repository = repository;
        _options = options;
    }
    
    public async Task<string?> Login(string email, string password)
    {
        var user = await _repository.FindByEmailAsync(email);
        
        if (user == null || !PasswordHasher.Verify(password + _options.Pepper, user.PasswordHash)) return null;

        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            signingCredentials: signingCredentials,
            claims: new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            },
            expires: DateTime.UtcNow.AddMinutes(_options.Expires)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task<IEnumerable<User>> GetAll(QueryFiltersData filters)
    {
        var users = _repository.FindAll().Where(x => x.Role != Role.Service);
        
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
            users = users.OrderByName(filters.SortBy, filters.SortOrder.Value);
        }
        else if (!string.IsNullOrWhiteSpace(filters.SortBy))
        {
            users = users.OrderByName(filters.SortBy);
        }
        else if (filters.SortOrder != null)
        {
            users = filters.SortOrder == SortOrder.Asc
                ? users.OrderBy(x => x.Id)
                : users.OrderByDescending(x => x.Id);
        }

        return await users.ToListAsync();
    }

    public async Task<User?> GetById(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);

        return (user?.Role == Role.Service ? null : user) ?? null;
    }

    private async Task<User> Register(RegisterData data, Role role)
    {
        if (await _repository.FindByEmailAsync(data.Email) != null)
            throw new BadRequestException("The email is already used");

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = data.Name,
            Surname = data.Surname,
            Patronymic = data.Patronymic,
            PhoneNumber = data.PhoneNumber,
            Email = data.Email,
            PasswordHash = PasswordHasher.HashPassword(data.Password + _options.Pepper),
            Role = role,
            IsBlocked = false
        };

        await _repository.AddUserAsync(user);

        return user;
    }

    public async Task<User> RegisterUser(RegisterData data)
    {
        return await Register(data, Role.User);
    }

    public async Task<User> RegisterManager(RegisterData data)
    {
        return await Register(data, Role.Manager);
    }

    public async Task EditSelf(Guid id, RegisterData data)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new NotFoundException("User not found");
        
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

        if (user == null) throw new NotFoundException("User not found");
        
        user.PasswordHash = PasswordHasher.HashPassword(password + _options.Pepper);

        await _repository.UpdateUserAsync(user);
    }

    public async Task ToggleUserBlock(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null || user.Role == Role.Service) throw new NotFoundException("User not found");
        user.IsBlocked = !user.IsBlocked;

        await _repository.UpdateUserAsync(user);
    }
}