using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationContext _context;
    
    public UsersRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<User> FindAll()
    {
        return _context.Users;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }
}