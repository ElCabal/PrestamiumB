using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbContext context;
    protected readonly IHttpContextAccessor httpContextAccessor;

    public BaseRepository(DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.httpContextAccessor = httpContextAccessor;
    }

    protected string GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Usuario no autenticado");
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync()
    {
        var userId = GetCurrentUserId();
        return await context.Set<TEntity>()
            .Where(x => x.Status)
            .WhereIf(HasUserProperty(), x => EF.Property<string>(x, "UserId") == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var userId = GetCurrentUserId();
        return await context.Set<TEntity>()
            .Where(x => x.Status)
            .WhereIf(HasUserProperty(), x => EF.Property<string>(x, "UserId") == userId)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        var userId = GetCurrentUserId();
        return await context.Set<TEntity>()
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Status &&
                (!HasUserProperty() || EF.Property<string>(x, "UserId") == userId));
    }

    public virtual async Task<int> CreateAsync(TEntity entity)
    {
        if (HasUserProperty())
        {
            var property = typeof(TEntity).GetProperty("UserId");
            property?.SetValue(entity, GetCurrentUserId());
        }

        await context.Set<TEntity>().AddAsync(entity);
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
            // GetByIdAsync ya valida que el usuario sea el propietario
            item.Status = false;
            await UpdateAsync();
        }
    }

    // Método helper para verificar si la entidad tiene propiedad UserId
    public bool HasUserProperty()
    {
        return typeof(TEntity).GetProperty("UserId") != null;
    }
}

// Extension method para Where condicional
public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}