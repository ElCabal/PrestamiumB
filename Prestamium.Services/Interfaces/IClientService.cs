using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface IClientService
    {
        Task<BaseResponseGeneric<int>> CreateAsync(ClientRequestDto request);
        Task<BaseResponseGeneric<ICollection<ClientResponseDto>>> GetAllAsync();
        Task<BaseResponseGeneric<ClientResponseDto>> GetByIdAsync(int id);
        Task<BaseResponseGeneric<ClientResponseDto>> GetByDocumentNumberAsync(string documentNumber);
    }
}
