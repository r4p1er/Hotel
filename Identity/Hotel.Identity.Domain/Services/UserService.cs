using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Hotel.Identity.Domain.Abstractions;
using Hotel.Identity.Domain.Entities;
using Hotel.Identity.Domain.Enums;
using Hotel.Identity.Domain.Models;
using Hotel.Identity.Domain.Utils;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.Identity.Domain.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService : IUserService
{
    /// <inheritdoc cref="IUsersRepository"/>
    private readonly IUsersRepository _repository;
    
    /// <inheritdoc cref="UserServiceOptions"/>
    private readonly UserServiceOptions _options;
    
    /// <summary>
    /// Валидатор регистрационных данных
    /// </summary>
    private readonly IValidator<RegisterData> _validator;

    public UserService(IUsersRepository repository, UserServiceOptions options, IValidator<RegisterData> validator)
    {
        _repository = repository;
        _options = options;
        _validator = validator;
    }
    
    /// <inheritdoc cref="IUserService.Login"/>
    public async Task<string> Login(string email, string password)
    {
        var user = await _repository.FindByEmailAsync(email);

        if (user == null || !PasswordHasher.Verify(password + _options.Pepper, user.PasswordHash))
            throw new NotFoundException("User not found");

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
    
    /// <inheritdoc cref="IUserService.GetAll"/>
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

    /// <inheritdoc cref="IUserService.GetById"/>
    public async Task<User> GetById(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null || user.Role == Role.Service) throw new NotFoundException("User not found");

        return user;
    }

    /// <summary>
    /// Подготовить объект нового пользователя
    /// </summary>
    /// <param name="data">Регистрационные данные</param>
    /// <param name="role">Роль пользователя</param>
    /// <returns>Пользователь</returns>
    /// <exception cref="BadRequestException">Исключение, если email уже используется</exception>
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

    /// <inheritdoc cref="IUserService.RegisterUser"/>
    public async Task<User> RegisterUser(RegisterData data)
    {
        await _validator.ValidateAndThrowAsync(data);
        
        return await Register(data, Role.User);
    }

    /// <inheritdoc cref="IUserService.RegisterManager"/>
    public async Task<User> RegisterManager(RegisterData data)
    {
        await _validator.ValidateAndThrowAsync(data);
        
        return await Register(data, Role.Manager);
    }

    /// <inheritdoc cref="IUserService.EditSelf"/>
    public async Task EditSelf(Guid id, RegisterData data)
    {
        await _validator.ValidateAndThrowAsync(data);
        
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new NotFoundException("User not found");
        
        user.Name = data.Name;
        user.Surname = data.Surname;
        user.Patronymic = data.Patronymic;
        user.PhoneNumber = data.PhoneNumber;
        user.Email = data.Email;

        await _repository.UpdateUserAsync(user);
    }

    /// <inheritdoc cref="IUserService.EditPassword"/>
    public async Task EditPassword(Guid id, string password)
    {
        var user = await _repository.FindByIdAsync(id);

        if (user == null) throw new NotFoundException("User not found");
        
        user.PasswordHash = PasswordHasher.HashPassword(password + _options.Pepper);

        await _repository.UpdateUserAsync(user);
    }

    /// <inheritdoc cref="IUserService.ToggleUserBlock"/>
    public async Task ToggleUserBlock(Guid id)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null || user.Role == Role.Service) throw new NotFoundException("User not found");
        user.IsBlocked = !user.IsBlocked;

        await _repository.UpdateUserAsync(user);
    }
}