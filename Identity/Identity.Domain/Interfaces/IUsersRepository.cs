using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces;

public interface IUsersRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    IQueryable<User> FindAll();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}