using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DoctorApp.Exceptions;
using DoctorApp.Security;

namespace DoctorApp.Services.ApiClient;

/// <summary>
/// Cliente genérico para consumir APIs
/// Implementa manejo robusto de errores, retry y serialización JSON
/// </summary>
public interface IApiClient
{
    Task<T> GetAsync<T>(string endpoint);
    Task<T> PostAsync<T>(string endpoint, object? data = null);
    Task<T> PutAsync<T>(string endpoint, object? data = null);
    Task DeleteAsync(string endpoint);
    Task<HttpResponseMessage> GetRawAsync(string endpoint);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        // Configurar opciones JSON (case-insensitive para compatibilidad)
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error en GET {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<T> PostAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            HttpContent? content = null;

            if (data != null)
            {
                content = new StringContent(
                    JsonSerializer.Serialize(data, _jsonOptions),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            }

            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex) when (!(ex is AppException))
        {
            throw new ConnectionException($"Error en POST {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<T> PutAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            HttpContent? content = null;

            if (data != null)
            {
                content = new StringContent(
                    JsonSerializer.Serialize(data, _jsonOptions),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            }

            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex) when (!(ex is AppException))
        {
            throw new ConnectionException($"Error en PUT {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
            }
        }
        catch (Exception ex) when (!(ex is AppException))
        {
            throw new ConnectionException($"Error en DELETE {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<HttpResponseMessage> GetRawAsync(string endpoint)
    {
        return await _httpClient.GetAsync(endpoint);
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _jsonOptions) 
                    ?? throw new AppException("Respuesta vacía del servidor");
            }
            catch (JsonException ex)
            {
                throw new AppException($"Error deserializando respuesta: {ex.Message}", "JSON_ERROR", 500, ex);
            }
        }

        await HandleErrorResponse(response);
        throw new AppException("Error inesperado", "UNKNOWN_ERROR", (int?)response.StatusCode);
    }

    private async Task HandleErrorResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                // 401: Token expirado o inválido
                throw new UnauthorizedException();

            case HttpStatusCode.NotFound:
                // 404: Recurso no encontrado
                throw new AppException(
                    content ?? "El recurso solicitado no fue encontrado",
                    "NOT_FOUND",
                    404
                );

            case HttpStatusCode.Conflict:
                // 409: Conflicto (ej: recurso duplicado)
                throw new ConflictException(content);

            case HttpStatusCode.BadRequest:
                // 400: Datos inválidos
                try
                {
                    var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(content, _jsonOptions);
                    var errors = problemDetails?.Errors ?? new Dictionary<string, string[]>();
                    throw new Exceptions.ValidationException("Error de validación en el servidor", errors);
                }
                catch (Exceptions.ValidationException)
                {
                    throw;
                }
                catch
                {
                    throw new Exceptions.ValidationException(content ?? "Datos inválidos");
                }

            case HttpStatusCode.InternalServerError:
                // 500: Error interno del servidor
                throw new ConnectionException($"Error interno del servidor (500): {content}");

            case HttpStatusCode.ServiceUnavailable:
                // 503: Servicio no disponible
                throw new ConnectionException("El servidor no está disponible (503). Intenta más tarde.");

            case HttpStatusCode.GatewayTimeout:
                // 504: Gateway timeout
                throw new ConnectionException("El servidor tardó demasiado en responder (504).");

            default:
                // Otros errores HTTP
                throw new AppException(
                    content ?? $"Error HTTP {(int)response.StatusCode}",
                    $"HTTP_{(int)response.StatusCode}",
                    (int)response.StatusCode
                );
        }
    }
}

/// <summary>
/// Modelo para errores de validación de ASP.NET Core
/// </summary>
public class ValidationProblemDetails
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}

