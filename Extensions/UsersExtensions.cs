using System.Reflection;
using Entities;

namespace Extensions;

public static partial class Extensions
{
    public static IEnumerable<User> FilterUsers(this IEnumerable<User> users, string filter, string filterValue)
    {
        if (filter is not null && filterValue is not null && typeof(User).GetProperties().Select(p => p.Name).Contains(filter))
            users = users.Where(ent => ent.GetType().GetProperty(filter)!.GetValue(ent)!.ToString()!.ToLower() == filterValue.ToString().ToLower());

        if (filter is not null && filter.Equals("Roles") && filterValue is not null)
            users = users.Where(ent => ((List<Role>)(ent.GetType().GetProperty("Status")!.GetValue(ent))!).Select(role => role.Access.ToString()).Contains(filterValue));

        return users;
    }
    public static IEnumerable<User> SortUsers(this IEnumerable<User> users, string sort, string order)
    {
        if (sort is not null && order is not null && typeof(User).GetProperties().Select(p => p.Name).Contains(sort))
            if (order.ToLower().Equals("asc"))
            {
                users = users.OrderBy(ent => ent.GetType().GetProperty(sort)!.GetValue(ent));
            }
            else if (order.ToLower().Equals("desc"))
            {
                users = users.OrderByDescending(ent => ent.GetType().GetProperty(sort)!.GetValue(ent));
            }
        return users;
    }
}