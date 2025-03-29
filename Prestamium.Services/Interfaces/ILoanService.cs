using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface ILoanService
    {
        Task<BaseResponseGeneric<int>> CreateAsync(LoanRequestDto request);
        Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetAllAsync();
        Task<BaseResponseGeneric<LoanResponseDto>> GetByIdAsync(int id);
        Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetByClientIdAsync(int clientId);
        Task<BaseResponseGeneric<LoanDetailResponseDto>> GetDetailAsync(int clientId);
        Task<BaseResponseGeneric<decimal>> CalculateLateFeesAsync(int installmentId, DateTime paymentDate);
        Task<BaseResponseGeneric<bool>> RegisterInstallmentPaymentAsync(int installmentId, decimal amount, int boxId);
        
        // Nuevo método para obtener préstamos paginados y filtrados
        Task<PaginatedResponseDto<LoanResponseDto>> GetPaginatedAsync(LoanFilterRequestDto filter);
    }
}
