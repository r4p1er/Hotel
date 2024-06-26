using System.Linq.Expressions;
using Hotel.Shared.Exceptions;
using Managing.Domain.Entities;
using Managing.Domain.Enums;

namespace Managing.Domain;

public static class QueryableExtensions
{
    public static IQueryable<Room> OrderByName(this IQueryable<Room> queryable, string name, SortOrder order = SortOrder.Asc)
    {
        var prop = typeof(Room).GetProperty(name);

        if (prop == null)
            throw new BadRequestException(
                "Name of prop to order is invalid. Consider write a name with a first letter being capital");
        
        var param = Expression.Parameter(typeof(Room), "x");
        var accessingProp = Expression.MakeMemberAccess(param, prop);
        var lambdaResult = Expression.Convert(accessingProp, typeof(object));
        var sortingExpression = Expression.Lambda<Func<Room, object>>(lambdaResult, param);

        return order == SortOrder.Asc
            ? queryable.OrderBy(sortingExpression)
            : queryable.OrderByDescending(sortingExpression);
    }
}