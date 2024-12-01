using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor
            ) : base(context, httpContextAccessor)
        {
            _context = context;
        }

        public async Task<Client?> GetByDocumentNumberAsync(string documentNumber)
        {
            return await _context.Set<Client>()
                .FirstOrDefaultAsync(x => x.DocumentNumber == documentNumber && x.Status);
        }
    }
}
