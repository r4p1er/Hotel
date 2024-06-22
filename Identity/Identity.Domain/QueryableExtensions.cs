using System.Linq.Expressions;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Exceptions;

namespace Identity.Domain;

public static class QueryableExtensions
{
    public static IQueryable<User> OrderByName(this IQueryable<User> queryable, string name,
        SortOrder order = SortOrder.Asc)
    {
        var prop = typeof(User).GetProperty(name);

        if (prop == null)
            throw new BadRequestException(
                "Name of prop to order is invalid. Consider write a name with a first letter being capital");
        
        var param = Expression.Parameter(typeof(User), "x");
        var accessingProp = Expression.MakeMemberAccess(param, prop);
        var lambdaResult = Expression.Convert(accessingProp, typeof(object));
        var sortingExpression = Expression.Lambda<Func<User, object>>(lambdaResult, param);

        return order == SortOrder.Asc
            ? queryable.OrderBy(sortingExpression)
            : queryable.OrderByDescending(sortingExpression);
    }
}