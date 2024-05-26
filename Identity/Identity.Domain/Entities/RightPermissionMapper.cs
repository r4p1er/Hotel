using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public static class RightPermissionMapper
{
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

    public static Permission RightToPermission(Role role, Right right)
    {
        return RolePermissionMap[role][right];
    }
}