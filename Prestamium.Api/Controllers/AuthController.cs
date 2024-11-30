using Microsoft.AspNetCore.Mvc;
using Prestamium.Dto.Request;
using Prestamium.Services.Interfaces;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Al igual que en tu LoanController, mantenemos la inyección de dependencias simple y directa
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Observa cómo simplificamos el método siguiendo tu patrón de respuesta ternaria
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Aplicamos la misma estructura limpia al login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}