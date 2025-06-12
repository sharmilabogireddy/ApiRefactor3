using System.Linq.Expressions;

namespace ApiRefactor.Repositories.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<T?> Find(object id);

        Task<bool> Any(Expression<Func<T, bool>> expression);

        Task<T?> Find(T entity);

        Task<IQueryable<T>> FindAll(bool hasChangesTracked = true);

        Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool hasChangesTracked = true);

        Task<T> Create(T entity);

        Task Create(T[] entity);

        Task<T> Update(T entity);

        void UpdateChanges(T entity);

        Task Update(T[] entities);

        Task<T> Delete(T entity);

        Task Delete(T[] entities);
    }
}
