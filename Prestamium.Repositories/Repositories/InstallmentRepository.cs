using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class InstallmentRepository : BaseRepository<Installment> , IInstallmentRepository
    {
        public InstallmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
