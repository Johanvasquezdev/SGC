using DoctorApp.DTOs.Requests;
using DoctorApp.DTOs.Responses;
using DoctorApp.Exceptions;
using DoctorApp.Services.ApiClient;
using DoctorApp.Services.Interfaces;
using DoctorApp.Security;

namespace DoctorApp.Services.Implementation;

/// <summary>
/// Servicio de Autenticación - Maneja Login/Logout
/// </summary>
public class AuthService : IAuthService
{
    private readonly IApiClient _apiClient;
    private readonly ITokenManager _tokenManager;
    private const string LoginEndpoint = "/api/auth/login";
    private const string RefreshEndpoint = "/api/auth/refresh";

    public AuthService(IApiClient apiClient, ITokenManager tokenManager)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    public async Task<AuthTokenResponse> LoginAsync(string usuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            throw new ValidationException("Usuario y contraseña son requeridos");

        try
        {
            var request = new LoginRequest
            {
                Usuario = usuario,
                Contrasena = contrasena
            };

            var response = await _apiClient.PostAsync<AuthTokenResponse>(LoginEndpoint, request);

            if (response == null || string.IsNullOrEmpty(response.AccessToken))
                throw new UnauthorizedException("Credenciales inválidas");

            // Guardar token de forma segura
            await _tokenManager.SaveTokenAsync(response.AccessToken);

            return response;
        }
        catch (UnauthorizedException)
        {
            throw;
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al iniciar sesión: {ex.Message}", ex);
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            // Limpiar token localmente
            await _tokenManager.RemoveTokenAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al cerrar sesión: {ex.Message}");
        }
    }

    public async Task<bool> EstaAutenticadoAsync()
    {
        return await _tokenManager.IsTokenValidAsync();
    }

    public async Task<AuthTokenResponse> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            var request = new RefreshTokenRequest { RefreshToken = refreshToken };
            var response = await _apiClient.PostAsync<AuthTokenResponse>(RefreshEndpoint, request);

            if (response?.AccessToken != null)
            {
                await _tokenManager.SaveTokenAsync(response.AccessToken);
            }

            return response ?? throw new UnauthorizedException("No se pudo renovar el token");
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al renovar token: {ex.Message}", ex);
        }
    }
}

/// <summary>
/// DTO para login request
/// </summary>
public class LoginRequest
{
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}

/// <summary>
/// DTO para refresh token request
/// </summary>
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
