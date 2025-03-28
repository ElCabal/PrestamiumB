using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Persistence;
using Prestamium.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<User> userManager,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

    public async Task<BaseResponseGeneric<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        var response = new BaseResponseGeneric<AuthResponseDto>();
        try
        {
            // Verificamos si el usuario ya existe
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                response.Success = false;
                response.ErrorMessage = "El usuario ya existe";
                return response;
            }

            // Creamos el nuevo usuario
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            // Intentamos crear el usuario
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            // Si todo sale bien, generamos el token y devolvemos la respuesta
            response.Data = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = GenerateJwtToken(user)
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Ocurrió un error al registrar el usuario";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<AuthResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var response = new BaseResponseGeneric<AuthResponseDto>();
        try
        {
            // Buscamos al usuario
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Success = false;
                response.ErrorMessage = "Usuario o contraseña incorrectos";
                return response;
            }

            // Verificamos la contraseña
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                response.Success = false;
                response.ErrorMessage = "Usuario o contraseña incorrectos";
                return response;
            }

            // Generamos un nuevo refresh token
            var refreshToken = await GenerateRefreshToken(user.Id);

            // Generamos el token JWT y la respuesta
            response.Data = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = GenerateJwtToken(user),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiryDate,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Ocurrió un error al iniciar sesión";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }
    private string GenerateJwtToken(User user)
    {
            // Obtenemos la clave secreta desde la configuración
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            // Creamos las credenciales de firma
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Creamos los claims (información que queremos incluir en el token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Configuramos el token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            // Generamos el token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
     }

    private async Task<RefreshToken> GenerateRefreshToken(string userId)
    {
        // El refresh token durará 7 veces más que el JWT
        var jwtDurationInMinutes = _configuration.GetValue<int>("JwtSettings:DurationInMinutes");
        var refreshTokenDurationInMinutes = jwtDurationInMinutes * 7;

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiryDate = DateTime.UtcNow.AddMinutes(refreshTokenDurationInMinutes),
            CreatedDate = DateTime.UtcNow,
            UserId = userId,
            IsRevoked = false
        };

        // Antes de crear uno nuevo, revocamos cualquier refresh token activo del usuario
        var existingTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in existingTokens)
        {
            token.IsRevoked = true;
        }

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<BaseResponseGeneric<AuthResponseDto>> RefreshTokenAsync(string refreshToken)
    {
        var response = new BaseResponseGeneric<AuthResponseDto>();
        try
        {
            // Buscamos el refresh token en la base de datos
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null)
            {
                response.Success = false;
                response.ErrorMessage = "Token de renovación inválido";
                return response;
            }

            // Verificamos si el token ha expirado
            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                storedToken.IsRevoked = true;
                await _context.SaveChangesAsync();

                response.Success = false;
                response.ErrorMessage = "Token de renovación expirado";
                return response;
            }

            // Generamos un nuevo refresh token
            var newRefreshToken = await GenerateRefreshToken(storedToken.UserId);

            // Generamos la respuesta con el nuevo JWT y refresh token
            response.Data = new AuthResponseDto
            {
                UserId = storedToken.User.Id,
                Email = storedToken.User.Email!,
                Token = GenerateJwtToken(storedToken.User),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiryDate,
                FirstName = storedToken.User.FirstName,
                LastName = storedToken.User.LastName
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorMessage = "Error al renovar el token";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        try
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null)
                return false;

            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
