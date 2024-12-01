using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class BoxTransactionRepository : BaseRepository<BoxTransaction>, IBoxTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public BoxTransactionRepository(
            ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor
            ) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        // Solo implementamos el método específico de IBoxTransactionRepository
        public async Task<IEnumerable<BoxTransaction>> GetTransactionsByBoxAsync(int boxId)
        {
            return await _context.Set<BoxTransaction>()
                .Where(x => x.BoxId == boxId)
                .OrderByDescending(x => x.TransactionDate)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
