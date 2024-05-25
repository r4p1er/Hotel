using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string PasswordHash { get; private set; }
    
    public Role Role { get; set; }
    
    public bool IsBlocked { get; set; }
    
    private static readonly Dictionary<Role, Dictionary<Right, Permission>> RolePermissionMap =
        new Dictionary<Role, Dictionary<Right, Permission>>()
        {
            {
                Role.User, new Dictionary<Right, Permission>()
                {
                    { Right.ReadUsers, Permission.NoAccess },
                    { Right.ReadUserById, Permission.NoAccess },
                    { Right.ReadSelf, Permission.Access },
                    { Right.CreateUser, Permission.NoAuthorize },
                    { Right.CreateManager, Permission.NoAccess },
                    { Right.UpdateSelf, Permission.Access },
                    { Right.UpdatePassword, Permission.Access },
                    { Right.BlockUser, Permission.NoAccess }
                }
            },
            {
                Role.Manager, new Dictionary<Right, Permission>()
                {
                    { Right.ReadUsers, Permission.NoAccess },
                    { Right.ReadUserById, Permission.NoAccess },
                    { Right.ReadSelf, Permission.Access },
                    { Right.CreateUser, Permission.NoAuthorize },
                    { Right.CreateManager, Permission.NoAccess },
                    { Right.UpdateSelf, Permission.Access },
                    { Right.UpdatePassword, Permission.Access },
                    { Right.BlockUser, Permission.NoAccess }
                }
            },
            {
                Role.Admin, new Dictionary<Right, Permission>()
                {
                    { Right.ReadUsers, Permission.Access },
                    { Right.ReadUserById, Permission.Access },
                    { Right.ReadSelf, Permission.Access },
                    { Right.CreateUser, Permission.NoAuthorize },
                    { Right.CreateManager, Permission.Access },
                    { Right.UpdateSelf, Permission.Access },
                    { Right.UpdatePassword, Permission.Access },
                    { Right.BlockUser, Permission.Access }
                }
            },
            {
                Role.Service, new Dictionary<Right, Permission>()
                {
                    { Right.ReadUsers, Permission.Access },
                    { Right.ReadUserById, Permission.Access },
                    { Right.ReadSelf, Permission.NoAccess },
                    { Right.CreateUser, Permission.NoAuthorize },
                    { Right.CreateManager, Permission.NoAccess },
                    { Right.UpdateSelf, Permission.NoAccess },
                    { Right.UpdatePassword, Permission.NoAccess },
                    { Right.BlockUser, Permission.NoAccess }
                }
            }
        };

    public void ChangePassword(string password)
    {
        throw new NotImplementedException();
    }

    public Permission HasRight(Right right)
    {
        return RolePermissionMap[Role][right];
    }
}