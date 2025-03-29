using Prestamium.Entities;
using System.Linq.Expressions;

namespace Prestamium.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<ICollection<TEntity>> GetAllAsync();
        Task<ICollection<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate);
        //Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy);
        Task<TEntity?> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task UpdateAsync();
        Task DeleteAsync(int id);
        
        // Nuevo método para paginación
        Task<(ICollection<TEntity> Items, int TotalCount)> GetPaginatedAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
    }
}
