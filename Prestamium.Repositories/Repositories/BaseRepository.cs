using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Prestamium.Repositories.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
        }
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>()
                .Where(x => x.Status)
                .AsNoTracking() // Hace más efeciente la consulta
                .ToListAsync();
        }
        public async Task<ICollection<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>()
                .Where(x => x.Status)
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }
        //public async Task<ICollection<TEntity>> GetAsync<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy)
        //{
        //    return await context.Set<TEntity>()
        //        .Where(predicate)
        //        .OrderBy(orderBy)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>()
                //.AsNoTracking()
                //.FirstOrDefaultAsync(x => x.Id == id);
                .FindAsync(id);
        }
        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>()
                .AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        public virtual async Task UpdateAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item is not null)
            {
                item.Status = false;
                await UpdateAsync();
            }
        }
    }
}
