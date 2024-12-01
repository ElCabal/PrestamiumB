using Microsoft.AspNetCore.Http;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class InstallmentRepository : BaseRepository<Installment> , IInstallmentRepository
    {
        public InstallmentRepository(
            ApplicationDbContext context, 
            IHttpContextAccessor httpContextAccessor
            ) : base(context, httpContextAccessor)
        {
        }
    }
}
