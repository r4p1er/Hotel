using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUsersRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}