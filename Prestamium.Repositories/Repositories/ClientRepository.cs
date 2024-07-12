using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
