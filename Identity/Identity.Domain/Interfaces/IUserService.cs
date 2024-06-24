using Identity.Domain.DataObjects;
using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces;

public interface IUserService
{
    Task<string?> Login(string email, string password);
    Task<IEnumerable<User>> GetAll(QueryFiltersData filters);
    Task<User?> GetById(Guid id);
    Task<User> RegisterUser(RegisterData data);
    Task<User> RegisterManager(RegisterData data);
    Task EditSelf(Guid id, RegisterData data);
    Task EditPassword(Guid id, string password);
    Task ToggleUserBlock(Guid id);
}