namespace Prestamium.Dto.Response
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
