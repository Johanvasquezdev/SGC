using FluentValidation;
using DoctorApp.DTOs.Requests;
using DoctorApp.DTOs.Responses;
using DoctorApp.Exceptions;
using DoctorApp.Services.ApiClient;
using DoctorApp.Services.Interfaces;
using DoctorApp.Validators;

namespace DoctorApp.Services.Implementation;

/// <summary>
/// Servicio de Doctor - Consume endpoints de /api/doctores
/// Maneja registro, obtención, actualización y consulta de citas
/// Implementa manejo robusto de errores HTTP (404, 500, etc.)
/// </summary>
public class DoctorService : IDoctorService
{
    private readonly IApiClient _apiClient;
    private readonly RegistrarDoctorValidator _registrarValidator;
    private int? _cachedDoctorId;

    public DoctorService(
        IApiClient apiClient,
        RegistrarDoctorValidator registrarValidator)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _registrarValidator = registrarValidator ?? throw new ArgumentNullException(nameof(registrarValidator));
    }

    /// <summary>
    /// Obtiene todas las citas de un doctor específico desde la API
    /// Maneja errores: 404 (doctor no existe), 500 (error servidor), conexión
    /// </summary>
    /// <param name="doctorId">ID del doctor para filtrar citas</param>
    /// <returns>Lista de citas del doctor, vacía si no hay citas</returns>
    public async Task<List<CitaResponseDto>> GetCitasByDoctorIdAsync(int doctorId)
    {
        if (doctorId <= 0)
            throw new AppException("Doctor ID inválido", "INVALID_DOCTOR_ID", 400);

        try
        {
            var endpoint = $"/api/doctores/{doctorId}/citas";
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Obteniendo citas para doctor {doctorId} desde {endpoint}");

            var response = await _apiClient.GetAsync<List<CitaResponseDto>>(endpoint);
            return response ?? new List<CitaResponseDto>();
        }
        catch (AppException ex) when (ex.Code == "NOT_FOUND")
        {
            // 404: Doctor no existe
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Error 404: Doctor {doctorId} no encontrado");
            throw new AppException(
                $"Doctor con ID {doctorId} no encontrado. Verifica que el doctor esté registrado.",
                "DOCTOR_NOT_FOUND",
                404
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("500"))
        {
            // 500: Error interno del servidor
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Error 500: Problema en el servidor al obtener citas");
            throw new ConnectionException(
                "Error en el servidor al obtener citas. Intenta más tarde.",
                ex
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("503"))
        {
            // 503: Servicio no disponible
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Error 503: Servidor no disponible");
            throw new ConnectionException(
                "El servidor de citas no está disponible. Intenta en unos momentos.",
                ex
            );
        }
        catch (ConnectionException)
        {
            // Otras errores de conexión
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error inesperado al obtener citas del doctor: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Registra un nuevo doctor en el sistema
    /// Valida los datos, realiza POST a /api/doctores y almacena el ID en cache
    /// Maneja errores: 400 (validación), 409 (duplicado), 500 (servidor)
    /// </summary>
    public async Task<DoctorResponseDto> RegistrarDoctorAsync(DoctorRequestDto datos)
    {
        // Validar datos
        var validationResult = await _registrarValidator.ValidateAsync(datos);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException("Error de validación en el registro", errors);
        }

        try
        {
            var endpoint = "/api/doctores/registrar";
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Registrando doctor: {datos.Nombre} {datos.Apellido}");
            var response = await _apiClient.PostAsync<DoctorResponseDto>(endpoint, datos);

            if (response == null || response.Id == 0)
                throw new AppException("No se recibió respuesta válida del servidor");

            // Cachear el ID del doctor registrado
            _cachedDoctorId = response.Id;

            System.Diagnostics.Debug.WriteLine($"[DoctorService] Doctor registrado exitosamente. ID: {response.Id}");
            return response;
        }
        catch (ConflictException ex)
        {
            // 409: Doctor ya existe (email duplicado)
            throw new AppException($"Doctor ya existe: {ex.Message}", "DOCTOR_EXISTS", 409);
        }
        catch (Exceptions.ValidationException)
        {
            throw;
        }
        catch (ConnectionException ex) when (ex.Message.Contains("500"))
        {
            // 500: Error interno del servidor
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Error 500 durante registro");
            throw new ConnectionException(
                "Error en el servidor al registrar doctor. Intenta más tarde.",
                ex
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("503"))
        {
            // 503: Servicio no disponible
            throw new ConnectionException(
                "El servidor no está disponible. Intenta en unos momentos.",
                ex
            );
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al registrar doctor: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Obtiene los datos del doctor actual usando su ID cacheado
    /// Maneja errores: 404 (doctor no existe), 500 (error servidor)
    /// </summary>
    public async Task<DoctorResponseDto> ObtenerDoctorActualAsync()
    {
        try
        {
            if (!_cachedDoctorId.HasValue)
                throw new AppException("Doctor ID no disponible. Por favor registra primero.", "NO_DOCTOR_ID", 400);

            var endpoint = $"/api/doctores/{_cachedDoctorId}";
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Obteniendo datos del doctor {_cachedDoctorId}");
            var response = await _apiClient.GetAsync<DoctorResponseDto>(endpoint);

            if (response == null)
                throw new AppException("Doctor no encontrado");

            return response;
        }
        catch (AppException ex) when (ex.Code == "NOT_FOUND")
        {
            // 404: Doctor no existe
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Error 404: Doctor no encontrado");
            throw new AppException(
                "Tu perfil de doctor no fue encontrado. Verifica que estés registrado.",
                "DOCTOR_NOT_FOUND",
                404
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("500"))
        {
            // 500: Error interno del servidor
            throw new ConnectionException(
                "Error en el servidor al obtener datos del doctor. Intenta más tarde.",
                ex
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("503"))
        {
            // 503: Servicio no disponible
            throw new ConnectionException(
                "El servidor no está disponible. Intenta en unos momentos.",
                ex
            );
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al obtener datos del doctor: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Actualiza los datos de un doctor
    /// Maneja errores: 400 (validación), 404 (doctor no existe), 500 (servidor)
    /// </summary>
    public async Task<DoctorResponseDto> ActualizarDoctorAsync(int doctorId, DoctorRequestDto datos)
    {
        if (doctorId <= 0)
            throw new AppException("Doctor ID inválido", "INVALID_DOCTOR_ID", 400);

        // Validar datos
        var validationResult = await _registrarValidator.ValidateAsync(datos);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException("Error de validación en la actualización", errors);
        }

        try
        {
            var endpoint = $"/api/doctores/{doctorId}";
            System.Diagnostics.Debug.WriteLine($"[DoctorService] Actualizando doctor {doctorId}");
            var response = await _apiClient.PutAsync<DoctorResponseDto>(endpoint, datos);

            if (response == null)
                throw new AppException("No se recibió respuesta válida del servidor");

            System.Diagnostics.Debug.WriteLine($"[DoctorService] Doctor {doctorId} actualizado exitosamente");
            return response;
        }
        catch (AppException ex) when (ex.Code == "NOT_FOUND")
        {
            // 404: Doctor no existe
            throw new AppException(
                $"El doctor con ID {doctorId} no fue encontrado.",
                "DOCTOR_NOT_FOUND",
                404
            );
        }
        catch (Exceptions.ValidationException)
        {
            throw;
        }
        catch (ConnectionException ex) when (ex.Message.Contains("500"))
        {
            // 500: Error interno del servidor
            throw new ConnectionException(
                "Error en el servidor al actualizar doctor. Intenta más tarde.",
                ex
            );
        }
        catch (ConnectionException ex) when (ex.Message.Contains("503"))
        {
            // 503: Servicio no disponible
            throw new ConnectionException(
                "El servidor no está disponible. Intenta en unos momentos.",
                ex
            );
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al actualizar doctor: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Establece el ID del doctor (útil después de login)
    /// </summary>
    public void EstablecerDoctorId(int doctorId)
    {
        _cachedDoctorId = doctorId;
    }

    /// <summary>
    /// Obtiene el ID del doctor cacheado
    /// </summary>
    public int? ObtenerDoctorIdCacheado()
    {
        return _cachedDoctorId;
    }
}
