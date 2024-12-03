using Prestamium.Dto.Request;
using Prestamium.Dto.Response;

namespace Prestamium.Services.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponseGeneric<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<BaseResponseGeneric<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<BaseResponseGeneric<AuthResponseDto>> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }
}
