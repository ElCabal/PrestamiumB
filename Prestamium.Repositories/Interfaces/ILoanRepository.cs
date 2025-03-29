using Prestamium.Dto.Request;
using Prestamium.Entities;

namespace Prestamium.Repositories.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task<ICollection<Loan>> GetLoansByClientAsync(int clientId);
        Task<Loan?> GetLoanWithInstallmentsAsync(int id);
        Task<Loan?> GetLoanWithDetailsAsync(int id);
        
        // Nuevo método para paginación y filtros de préstamos
        Task<(ICollection<Loan> Items, int TotalCount)> GetPaginatedLoansAsync(LoanFilterRequestDto filter);
    }
}
