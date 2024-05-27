using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUsersRepository
{
    Task<User?> FindByEmailAsync(string email);
}