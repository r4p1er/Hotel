using Hotel.Identity.Domain.Abstractions;
using Hotel.Identity.Domain.Entities;
using Hotel.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Identity.Infrastructure.Services;

/// <inheritdoc cref="IUsersRepository"/>
public class UsersRepository(ApplicationContext context) : IUsersRepository
{
    /// <inheritdoc cref="IUsersRepository.FindByEmailAsync"/>
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    /// <inheritdoc cref="IUsersRepository.FindByIdAsync"/>
    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc cref="IUsersRepository.FindAll"/>
    public IQueryable<User> FindAll()
    {
        return context.Users;
    }

    /// <inheritdoc cref="IUsersRepository.AddUserAsync"/>
    public async Task AddUserAsync(User user)
    {
        await context.AddAsync(user);

        await context.SaveChangesAsync();
    }

    /// <inheritdoc cref="IUsersRepository.UpdateUserAsync"/>
    public async Task UpdateUserAsync(User user)
    {
        context.Entry(user).State = EntityState.Modified;

        await context.SaveChangesAsync();
    }
}