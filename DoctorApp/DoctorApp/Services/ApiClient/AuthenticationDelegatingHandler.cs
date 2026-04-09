using DoctorApp.Exceptions;
using DoctorApp.Security;

namespace DoctorApp.Services.ApiClient;

/// <summary>
/// Middleware que intercepta todas las solicitudes HTTP y agrega el token
/// </summary>
public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly ITokenManager _tokenManager;
    private int _retryCount = 0;
    private const int MaxRetries = 3;

    public AuthenticationDelegatingHandler(ITokenManager tokenManager)
    {
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Obtener token
        var token = await _tokenManager.GetTokenAsync();

        if (!string.IsNullOrEmpty(token))
        {
            // Inyectar token en header
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        // Agregar header de idempotencia (Idempotency-Key)
        if (!request.Headers.Contains("Idempotency-Key"))
        {
            request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());
        }

        var response = await base.SendAsync(request, cancellationToken);

        // Manejo de errores específicos
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Token expirado
            await _tokenManager.RemoveTokenAsync();
            throw new UnauthorizedException("Tu token ha expirado. Por favor vuelve a iniciar sesión.");
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            throw new ConflictException("Conflicto en la operación. El recurso ya existe o está siendo usado.");
        }

        // Retry para errores transitorios (500, 503, timeout)
        if ((int)response.StatusCode >= 500 && _retryCount < MaxRetries)
        {
            _retryCount++;
            await Task.Delay(1000 * _retryCount, cancellationToken); // Backoff exponencial
            return await SendAsync(request, cancellationToken);
        }

        return response;
    }
}

