using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface ILoanService
    {
        Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetAsync();
        Task<BaseResponseGeneric<LoanResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(LoanRequestDto request);
    }
}
