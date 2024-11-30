using Prestamium.Entities;

namespace Prestamium.Repositories.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task<ICollection<Loan>> GetLoansByClientAsync(int clientId);
        Task<Loan?> GetLoanWithDetailsAsync(int id);
    }
}
