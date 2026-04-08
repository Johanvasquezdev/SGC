namespace DoctorApp.Security;

/// <summary>
/// Gestor de tokens JWT usando SecureStorage
/// Implementa almacenamiento seguro del token de autenticación
/// </summary>
public interface ITokenManager
{
    Task<string?> GetTokenAsync();
    Task SaveTokenAsync(string token);
    Task RemoveTokenAsync();
    Task<bool> IsTokenValidAsync();
    bool IsTokenExpired(string token);
}

public class TokenManager : ITokenManager
{
    private const string TOKEN_KEY = "auth_token";
    private const string TOKEN_EXPIRY_KEY = "token_expiry";

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync(TOKEN_KEY);
            if (string.IsNullOrEmpty(token))
                return null;

            // Verificar si el token está expirado
            if (IsTokenExpired(token))
            {
                await RemoveTokenAsync();
                return null;
            }

            return token;
        }
        catch
        {
            return null;
        }
    }

    public async Task SaveTokenAsync(string token)
    {
        try
        {
            await SecureStorage.SetAsync(TOKEN_KEY, token);

            // Guardar fecha de expiración (ejemplo: 24 horas desde ahora)
            var expiryTime = DateTime.UtcNow.AddHours(24).ToString("O");
            await SecureStorage.SetAsync(TOKEN_EXPIRY_KEY, expiryTime);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving token: {ex.Message}");
            throw;
        }
    }

    public async Task RemoveTokenAsync()
    {
        try
        {
            SecureStorage.Remove(TOKEN_KEY);
            SecureStorage.Remove(TOKEN_EXPIRY_KEY);
        }
        catch
        {
            // Ya está vacío
        }
    }

    public async Task<bool> IsTokenValidAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token) && !IsTokenExpired(token);
    }

    /// <summary>
    /// Decodifica el JWT y verifica si está expirado (sin validar firma)
    /// </summary>
    public bool IsTokenExpired(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                return true;

            var payload = parts[1];
            // Agregar padding si es necesario
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

            var decodedBytes = Convert.FromBase64String(payload);
            var decodedPayload = System.Text.Encoding.UTF8.GetString(decodedBytes);

            // Buscar "exp" en el payload
            if (System.Text.Json.JsonDocument.Parse(decodedPayload).RootElement.TryGetProperty("exp", out var expElement))
            {
                var expirationUnix = expElement.GetInt64();
                var expirationDate = UnixTimeStampToDateTime(expirationUnix);

                return DateTime.UtcNow >= expirationDate;
            }

            return false;
        }
        catch
        {
            return true; // Asumir expirado si hay error
        }
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}
