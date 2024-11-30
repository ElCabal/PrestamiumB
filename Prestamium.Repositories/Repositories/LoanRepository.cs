using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
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

        public async Task<Loan?> GetLoanWithDetailsAsync(int id)
        {
            return await _context.Set<Loan>()
                .Include(x => x.Client)
                .Include(x => x.Box)
                .Include(x => x.Installments)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status);
        }
    }
}
