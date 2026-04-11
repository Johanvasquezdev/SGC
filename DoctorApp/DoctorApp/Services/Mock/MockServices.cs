using DoctorApp.DTOs.Requests;
using DoctorApp.DTOs.Responses;
using DoctorApp.Exceptions;
using DoctorApp.Services.Interfaces;

namespace DoctorApp.Services.Mock;

/// <summary>
/// Mock services (only for offline testing)
/// </summary>
public class MockCitasService : ICitasService
{
    public Task<ObtenerAgendaResponse> ObtenerAgendaAsync(DateTime fecha)
    {
        return Task.FromResult(new ObtenerAgendaResponse
        {
            Fecha = fecha,
            Citas = new List<CitaResponseDto>(),
            TotalCitas = 0,
            CitasConfirmadas = 0,
            CitasPendientes = 0
        });
    }

    public Task<CitaResponseDto> ConfirmarCitaAsync(int citaId, bool confirmada, string? notas = null)
    {
        return Task.FromResult(new CitaResponseDto { Id = citaId, Estado = confirmada ? "Confirmada" : "Cancelada" });
    }

    public Task<CitaResponseDto> IniciarConsultaAsync(int citaId)
    {
        return Task.FromResult(new CitaResponseDto { Id = citaId, Estado = "EnConsulta" });
    }

    public Task<CitaResponseDto> MarcarAsistenciaAsync(int citaId, bool asistio, string? observaciones = null)
    {
        return Task.FromResult(new CitaResponseDto { Id = citaId, Estado = asistio ? "Completada" : "NoAsistio" });
    }

    public Task<List<CitaResponseDto>> ObtenerCitasDelDiaAsync()
    {
        return Task.FromResult(new List<CitaResponseDto>());
    }

    public Task<CitaResponseDto?> ObtenerCitaPorIdAsync(int citaId)
    {
        return Task.FromResult<CitaResponseDto?>(new CitaResponseDto { Id = citaId });
    }
}

public class MockDisponibilidadService : IDisponibilidadService
{
    public Task<List<DisponibilidadResponseDto>> ObtenerDisponibilidadesAsync(int medicoId)
    {
        return Task.FromResult(new List<DisponibilidadResponseDto>());
    }

    public Task<DisponibilidadResponseDto> CrearDisponibilidadAsync(CrearDisponibilidadRequest request)
    {
        return Task.FromResult(new DisponibilidadResponseDto
        {
            Id = 1,
            MedicoId = request.MedicoId,
            DiaSemana = request.DiaSemana.ToString(),
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            DuracionCitaMin = request.DuracionCitaMin,
            EsRecurrente = request.EsRecurrente
        });
    }

    public Task<DisponibilidadResponseDto> ActualizarDisponibilidadAsync(ActualizarDisponibilidadRequest request)
    {
        return Task.FromResult(new DisponibilidadResponseDto
        {
            Id = request.DisponibilidadId,
            MedicoId = request.MedicoId,
            DiaSemana = request.DiaSemana.ToString(),
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            DuracionCitaMin = request.DuracionCitaMin,
            EsRecurrente = request.EsRecurrente
        });
    }

    public Task EliminarDisponibilidadAsync(int disponibilidadId) => Task.CompletedTask;

    public Task<DisponibilidadResponseDto?> ObtenerDisponibilidadPorIdAsync(int disponibilidadId)
    {
        return Task.FromResult<DisponibilidadResponseDto?>(new DisponibilidadResponseDto { Id = disponibilidadId });
    }
}

public class MockAuthService : IAuthService
{
    public Task<AuthTokenResponse> LoginAsync(string usuario, string contrasena)
    {
        if (usuario == "doctor" && contrasena == "password")
        {
            return Task.FromResult(new AuthTokenResponse
            {
                Token = "mock",
                NombreUsuario = "Doctor Demo",
                Rol = "Medico",
                Expiracion = DateTime.UtcNow.AddHours(1)
            });
        }

        throw new UnauthorizedException("Credenciales invalidas");
    }

    public Task LogoutAsync() => Task.CompletedTask;

    public Task<bool> EstaAutenticadoAsync() => Task.FromResult(true);
}

public class MockDoctorService : IDoctorService
{
    public Task<DoctorResponseDto> RegistrarDoctorAsync(DoctorRequestDto datos)
    {
        return Task.FromResult(new DoctorResponseDto
        {
            Id = 1,
            Nombre = $"{datos.Nombre} {datos.Apellido}".Trim(),
            Email = datos.Email ?? string.Empty,
            Especialidad = datos.Especialidad,
            TelefonoConsultorio = datos.Telefono,
            Activo = true,
            MedicoActivo = true
        });
    }

    public Task<DoctorResponseDto> ObtenerDoctorActualAsync()
    {
        throw new UnauthorizedException("No hay doctor registrado");
    }

    public Task<DoctorResponseDto> ActualizarDoctorAsync(int doctorId, DoctorRequestDto datos)
    {
        return Task.FromResult(new DoctorResponseDto
        {
            Id = doctorId,
            Nombre = $"{datos.Nombre} {datos.Apellido}".Trim(),
            Email = datos.Email ?? string.Empty,
            Especialidad = datos.Especialidad,
            TelefonoConsultorio = datos.Telefono,
            Activo = true,
            MedicoActivo = true
        });
    }

    public Task<List<CitaResponseDto>> GetCitasByDoctorIdAsync(int doctorId)
    {
        return Task.FromResult(new List<CitaResponseDto>());
    }

    public void EstablecerDoctorId(int doctorId)
    {
    }

    public int? ObtenerDoctorIdCacheado() => null;
}
