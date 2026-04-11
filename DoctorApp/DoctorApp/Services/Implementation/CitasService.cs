using FluentValidation;
using DoctorApp.DTOs.Requests;
using DoctorApp.DTOs.Responses;
using DoctorApp.Exceptions;
using DoctorApp.Services.ApiClient;
using DoctorApp.Services.Interfaces;
using DoctorApp.Validators;

namespace DoctorApp.Services.Implementation;

/// <summary>
/// Citas service - consumes /api/citas endpoints
/// </summary>
public class CitasService : ICitasService
{
    private readonly IApiClient _apiClient;
    private readonly ConfirmarCitaValidator _confirmarValidator;
    private readonly IniciarConsultaValidator _iniciarValidator;
    private readonly MarcarAsistenciaValidator _asistenciaValidator;

    public CitasService(
        IApiClient apiClient,
        ConfirmarCitaValidator confirmarValidator,
        IniciarConsultaValidator iniciarValidator,
        MarcarAsistenciaValidator asistenciaValidator)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _confirmarValidator = confirmarValidator;
        _iniciarValidator = iniciarValidator;
        _asistenciaValidator = asistenciaValidator;
    }

    public async Task<ObtenerAgendaResponse> ObtenerAgendaAsync(DateTime fecha)
    {
        try
        {
            var endpoint = $"/api/citas/medico/agenda?fecha={fecha:yyyy-MM-dd}";
            var citas = await _apiClient.GetAsync<List<CitaResponseDto>>(endpoint)
                ?? new List<CitaResponseDto>();

            var confirmadas = citas.Count(c =>
                c.Estado.Equals("Confirmada", StringComparison.OrdinalIgnoreCase) ||
                c.Estado.Equals("Completada", StringComparison.OrdinalIgnoreCase));

            var pendientes = citas.Count(c =>
                c.Estado.Equals("Solicitada", StringComparison.OrdinalIgnoreCase) ||
                c.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));

            return new ObtenerAgendaResponse
            {
                Fecha = fecha,
                Citas = citas,
                TotalCitas = citas.Count,
                CitasConfirmadas = confirmadas,
                CitasPendientes = pendientes
            };
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al obtener agenda: {ex.Message}", ex);
        }
    }

    public async Task<CitaResponseDto> ConfirmarCitaAsync(int citaId, bool confirmada, string? notas = null)
    {
        var request = new ConfirmarCitaRequest
        {
            CitaId = citaId,
            Confirmada = confirmada,
            Notas = notas
        };

        var validationResult = await _confirmarValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException("Error de validacion", errors);
        }

        try
        {
            var endpoint = $"/api/citas/{citaId}/confirmar";
            var response = await _apiClient.PutAsync<CitaResponseDto>(endpoint, request);
            return response ?? new CitaResponseDto { Id = citaId, Estado = "Confirmada" };
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al confirmar cita: {ex.Message}", ex);
        }
    }

    public async Task<CitaResponseDto> IniciarConsultaAsync(int citaId)
    {
        var request = new IniciarConsultaRequest { CitaId = citaId };

        var validationResult = await _iniciarValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException("Error de validacion", errors);
        }

        try
        {
            var endpoint = $"/api/citas/{citaId}/iniciar-consulta";
            var response = await _apiClient.PutAsync<CitaResponseDto>(endpoint, request);
            return response ?? new CitaResponseDto { Id = citaId, Estado = "EnProgreso" };
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al iniciar consulta: {ex.Message}", ex);
        }
    }

    public async Task<CitaResponseDto> MarcarAsistenciaAsync(int citaId, bool asistio, string? observaciones = null)
    {
        var request = new MarcarAsistenciaRequest
        {
            CitaId = citaId,
            Asistio = asistio,
            Observaciones = observaciones
        };

        var validationResult = await _asistenciaValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException("Error de validacion", errors);
        }

        try
        {
            var endpoint = $"/api/citas/{citaId}/asistencia?asistio={asistio}";
            var response = await _apiClient.PutAsync<CitaResponseDto>(endpoint, request);
            return response ?? new CitaResponseDto { Id = citaId, Estado = asistio ? "Completada" : "NoAsistio" };
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al marcar asistencia: {ex.Message}", ex);
        }
    }

    public async Task<List<CitaResponseDto>> ObtenerCitasDelDiaAsync()
    {
        try
        {
            var agenda = await ObtenerAgendaAsync(DateTime.Now.Date);
            return agenda.Citas ?? new List<CitaResponseDto>();
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al obtener citas del dia: {ex.Message}", ex);
        }
    }

    public async Task<CitaResponseDto?> ObtenerCitaPorIdAsync(int citaId)
    {
        try
        {
            var endpoint = $"/api/citas/{citaId}";
            return await _apiClient.GetAsync<CitaResponseDto>(endpoint);
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException($"Error al obtener cita: {ex.Message}", ex);
        }
    }
}
