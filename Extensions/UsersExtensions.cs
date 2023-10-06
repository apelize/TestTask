using Entities;

#pragma warning disable 1591

namespace Extensions;

public static partial class Extensions
{
    public static IEnumerable<User> FilterUsers(this IEnumerable<User> users, string filter, string filterValue)
    {
        if (filter is not null && filterValue is not null && typeof(User).GetProperties().Select(p => p.Name).Contains(filter))
            users = users.Where(user => 
                user.GetType().GetProperty(filter)!.GetValue(user)!.ToString()!.ToLower()
                .Equals(filterValue.ToString().ToLower()));

        if (filter is not null && filter.Equals("Roles") && filterValue is not null)
            users = users.Where(user => 
            ((List<Role>)(user.GetType().GetProperty("Status")!.GetValue(user))!)
            .Select(role => role.Access.ToString()).Contains(filterValue));

        return users;
    }
    public static IEnumerable<User> SortUsers(this IEnumerable<User> users, string sort, string order)
    {
        if (sort is not null && order is not null && typeof(User).GetProperties().Select(p => p.Name).Contains(sort))
            if (order.ToLower().Equals("asc"))
            {
                users = users.OrderBy(user => user.GetType().GetProperty(sort)!.GetValue(user));
            }
            else if (order.ToLower().Equals("desc"))
            {
                users = users.OrderByDescending(user => user.GetType().GetProperty(sort)!.GetValue(user));
            }
        return users;
    }
}