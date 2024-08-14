using System.Linq.Expressions;
using Hotel.Identity.Domain.Entities;
using Hotel.Identity.Domain.Enums;
using Hotel.Shared.Exceptions;

namespace Hotel.Identity.Domain;

/// <summary>
/// Расширения для IQueryable
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Отсортировать по выбранному полю
    /// </summary>
    /// <param name="queryable">IQueryable с пользователями</param>
    /// <param name="name">Название поля для сортировки</param>
    /// <param name="order">Порядок сортировки</param>
    /// <returns>Отсортированный IQueryable с пользователями</returns>
    /// <exception cref="BadRequestException">Исключение 400 Bad Request, если название поля для сортировки неверное</exception>
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