using Prestamium.Entities;

namespace Prestamium.Repositories.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task<ICollection<Loan>> GetLoansByClientAsync(int clientId);
        Task<Loan?> GetLoanWithInstallmentsAsync(int id);
        Task<Loan?> GetLoanWithDetailsAsync(int id);
    }
}
