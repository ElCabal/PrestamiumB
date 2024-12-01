using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

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
    }
}
