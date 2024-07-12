using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface IClientService
    {
        Task<BaseResponseGeneric<ICollection<ClientResponseDto>>> GetAsync();
        Task<BaseResponseGeneric<ClientResponseDto>> GetAsync(int id);
        Task<BaseResponseGeneric<int>> AddAsync(ClientRequestDto request);
        Task<BaseResponse> UpdateAsync(int id, ClientRequestDto request);
        Task<BaseResponse> DeleteAsync(int id);
    }
}
