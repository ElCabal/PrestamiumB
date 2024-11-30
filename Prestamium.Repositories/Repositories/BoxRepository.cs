using Prestamium.Persistence;
using Prestamium.Repositories.Interfaces;

namespace Prestamium.Repositories.Repositories
{
    public class BoxRepository : BaseRepository<Box>, IBoxRepository
    {
        public BoxRepository(ApplicationDbContext context) : base(context) 
        {
        }
    }
}
