using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface IBoxService
    {
        Task<BaseResponseGeneric<int>> CreateAsync(BoxRequestDto request);  // antes AddAsync
        Task<BaseResponseGeneric<ICollection<BoxResponseDto>>> GetAllAsync();  // se mantiene igual
        Task<BaseResponseGeneric<BoxResponseDto>> GetByIdAsync(int id);  // se mantiene igual
        Task<BaseResponseGeneric<int>> CreateTransactionAsync(BoxTransactionRequestDto request);  // antes AddTransactionAsync
    }
}
