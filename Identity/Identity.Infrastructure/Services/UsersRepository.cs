using Identity.Application.Interfaces;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services;

public class UsersRepository : IUsersRepository
{
    private readonly Dictionary<int, Domain.Enums.Role> _databaseDomainRoleMap = new Dictionary<int, Domain.Enums.Role>()
    {
        { 1, Domain.Enums.Role.User },
        { 2, Domain.Enums.Role.Manager },
        { 3, Domain.Enums.Role.Admin },
        { 4, Domain.Enums.Role.Service }
    };
    private readonly ApplicationContext _context;
    
    public UsersRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Entities.User?> FindByEmailAsync(string email)
    {
        return DbUserToDomainUser(await _context.Users.FirstOrDefaultAsync(x => x.Email == email));
    }

    public async Task<Domain.Entities.User?> FindByIdAsync(Guid id)
    {
        return DbUserToDomainUser(await _context.Users.FirstOrDefaultAsync(x => x.Id == id));
    }

    public IQueryable<Domain.Entities.User> FindAll()
    {
        return _context.Users.Select(x => DbUserToDomainUser(x)!);
    }

    public async Task AddUserAsync(Domain.Entities.User user)
    {
        var dbUser = DomainUserToDbUser(user);
        await _context.AddAsync(dbUser!);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(Domain.Entities.User user)
    {
        var dbUser = DomainUserToDbUser(user);
        _context.Entry(dbUser!).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    private Domain.Entities.User? DbUserToDomainUser(Database.Entities.User? user)
    {
        return user == null
            ? null
            : new Domain.Entities.User()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = user.PasswordHash,
                Role = _databaseDomainRoleMap[user.RoleId],
                IsBlocked = user.IsBlocked
            };
    }

    private Database.Entities.User? DomainUserToDbUser(Domain.Entities.User? user)
    {
        return user == null
            ? null
            : new Database.Entities.User()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = user.PasswordHash,
                RoleId = _databaseDomainRoleMap.First(x => x.Value == user.Role).Key,
                IsBlocked = user.IsBlocked
            };
    }
}