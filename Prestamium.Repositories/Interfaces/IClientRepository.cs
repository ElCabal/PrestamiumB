using Prestamium.Entities;

namespace Prestamium.Repositories.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Task<Client?> GetByDocumentNumberAsync(string documentNumber);
    }
}
