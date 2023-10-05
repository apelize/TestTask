using System.Linq.Expressions;

namespace Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> Get(int id);
    Task<IEnumerable<TEntity>> GetAll(string filter, string filterValue, string sort, string order, int pageIndex, int pageSize);
    Task Delete(int id);
    Task<TEntity> Find(Func<TEntity, bool> predicate);
}