using System.Linq.Expressions;

namespace Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> Get(int id);
    Task<IEnumerable<TEntity>> GetAll(string filter = null!, string filterValue = null!, string sort = null!, string order = null!, int pageIndex = 1, int pageSize = 10);
    Task Delete(int id);
    Task<TEntity> Find(Func<TEntity, bool> predicate);
}