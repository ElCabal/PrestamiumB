using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface IBoxService
    {
        Task<BaseResponseGeneric<int>> CreateAsync(BoxRequestDto request);
        Task<BaseResponseGeneric<ICollection<BoxResponseDto>>> GetAllAsync();
        Task<BaseResponseGeneric<BoxResponseDto>> GetByIdAsync(int id);
        Task<BaseResponseGeneric<BoxDetailResponseDto>> GetDetailAsync(int id);
        Task<BaseResponseGeneric<int>> CreateTransactionAsync(BoxTransactionRequestDto request);
        Task<BaseResponseGeneric<ICollection<BoxTransactionResponseDto>>> GetTransactionsByBoxIdAsync(int boxId);
    }
}
