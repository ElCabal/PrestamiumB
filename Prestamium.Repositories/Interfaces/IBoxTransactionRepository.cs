using Prestamium.Entities;

namespace Prestamium.Repositories.Interfaces
{
    public interface IBoxTransactionRepository : IBaseRepository<BoxTransaction>
    {
        Task<IEnumerable<BoxTransaction>> GetTransactionsByBoxAsync(int boxId);
    }
}
