using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Dto.Request;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;
using System.Security.Claims;

namespace Prestamium.Repositories.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(
            ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor
            ) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        public override async Task<ICollection<Loan>> GetAllAsync()
        {
            var userId = GetCurrentUserId();
            return await context.Set<Loan>()
                .Where(x => x.Status)
                .Include(x => x.Client)
                .WhereIf(HasUserProperty(), x => EF.Property<string>(x, "UserId") == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<Loan>> GetLoansByClientAsync(int clientId)
        {
            return await _context.Set<Loan>()
                .Where(x => x.ClientId == clientId && x.Status)
                .Include(x => x.Client)
                .Include(x => x.Box)
                .Include(x => x.Installments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Loan?> GetLoanWithInstallmentsAsync(int id)
        {
            var userId = GetCurrentUserId();
            return await context.Set<Loan>()
                .Include(l => l.Installments)
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
        }

        public async Task<Loan?> GetLoanWithDetailsAsync(int id)
        {
            var userId = GetCurrentUserId();
            return await context.Set<Loan>()
                .Include(l => l.Installments)
                .Include(l => l.Client)
                .Include(l => l.Box)
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
        }
        
        public async Task<(ICollection<Loan> Items, int TotalCount)> GetPaginatedLoansAsync(LoanFilterRequestDto filter)
        {
            var userId = GetCurrentUserId();
            
            // Construir la consulta base
            var query = _context.Set<Loan>()
                .Where(x => x.Status)
                .Include(x => x.Client)
                .Include(x => x.Box)
                .WhereIf(HasUserProperty(), x => EF.Property<string>(x, "UserId") == userId);
            
            // Aplicar filtros
            if (filter.ClientId.HasValue)
            {
                query = query.Where(x => x.ClientId == filter.ClientId.Value);
            }
            
            if (filter.MinAmount.HasValue)
            {
                query = query.Where(x => x.Amount >= filter.MinAmount.Value);
            }
            
            if (filter.MaxAmount.HasValue)
            {
                query = query.Where(x => x.Amount <= filter.MaxAmount.Value);
            }
            
            if (filter.StartDateFrom.HasValue)
            {
                query = query.Where(x => x.StartDate >= filter.StartDateFrom.Value);
            }
            
            if (filter.StartDateTo.HasValue)
            {
                query = query.Where(x => x.StartDate <= filter.StartDateTo.Value);
            }
            
            if (!string.IsNullOrEmpty(filter.Frequency))
            {
                query = query.Where(x => x.Frequency.ToLower() == filter.Frequency.ToLower());
            }
            
            if (filter.IsPaid.HasValue)
            {
                query = query.Where(x => (x.RemainingBalance == 0) == filter.IsPaid.Value);
            }
            
            // Obtener el conteo total para la paginación
            var totalCount = await query.CountAsync();
            
            // Aplicar ordenamiento por Id descendente
            var orderedQuery = query.OrderByDescending(e => e.Id);
            
            // Aplicar paginación
            var items = await orderedQuery
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToListAsync();
                
            return (items, totalCount);
        }
    }
}
