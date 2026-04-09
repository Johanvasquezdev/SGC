using DoctorApp.DTOs.Requests;
using DoctorApp.DTOs.Responses;
using DoctorApp.Services.Interfaces;
using DoctorApp.Exceptions;

namespace DoctorApp.Services.Mock;

/// <summary>
/// Servicio Mock de Citas para testing sin backend real
/// </summary>
public class MockCitasService : ICitasService
{
    public async Task<ObtenerAgendaResponse> ObtenerAgendaAsync(DateTime fecha)
    {
        // Simular delay de red
        await Task.Delay(500);

        return new ObtenerAgendaResponse
        {
            Fecha = fecha,
            Citas = new List<CitaResponseDto>
            {
                new CitaResponseDto
                {
                    Id = 1,
                    FechaHora = fecha.AddHours(9),
                    PacienteId = 101,
                    PacienteNombre = "Carlos López",
                    PacienteCedula = "1234567890",
                    Motivo = "Revisión general",
                    Confirmada = true,
                    Asistio = false,
                    DuracionMinutos = 30,
                    Estado = "Confirmada"
                },
                new CitaResponseDto
                {
                    Id = 2,
                    FechaHora = fecha.AddHours(10),
                    PacienteId = 102,
                    PacienteNombre = "María García",
                    PacienteCedula = "0987654321",
                    Motivo = "Seguimiento",
                    Confirmada = false,
                    Asistio = false,
                    DuracionMinutos = 30,
                    Estado = "Pendiente"
                }
            },
            TotalCitas = 2,
            CitasConfirmadas = 1,
            CitasPendientes = 1
        };
    }

    public async Task<CitaResponseDto> ConfirmarCitaAsync(int citaId, bool confirmada, string? notas = null)
    {
        await Task.Delay(300);
        return new CitaResponseDto
        {
            Id = citaId,
            Confirmada = confirmada,
            Estado = confirmada ? "Confirmada" : "Cancelada"
        };
    }

    public async Task<CitaResponseDto> IniciarConsultaAsync(int citaId)
    {
        await Task.Delay(300);
        return new CitaResponseDto { Id = citaId, Estado = "En Consulta" };
    }

    public async Task<CitaResponseDto> MarcarAsistenciaAsync(int citaId, bool asistio, string? observaciones = null)
    {
        await Task.Delay(300);
        return new CitaResponseDto { Id = citaId, Asistio = asistio };
    }

    public async Task<List<CitaResponseDto>> ObtenerCitasDelDiaAsync()
    {
        await Task.Delay(500);
        return new List<CitaResponseDto>
        {
            new CitaResponseDto
            {
                Id = 1,
                FechaHora = DateTime.Now.AddHours(1),
                PacienteId = 101,
                PacienteNombre = "Carlos López",
                PacienteCedula = "1234567890",
                Motivo = "Revisión general",
                Confirmada = true,
                DuracionMinutos = 30,
                Estado = "Confirmada"
            }
        };
    }

    public async Task<CitaResponseDto?> ObtenerCitaPorIdAsync(int citaId)
    {
        await Task.Delay(300);
        return new CitaResponseDto
        {
            Id = citaId,
            FechaHora = DateTime.Now,
            PacienteNombre = "Mock Patient"
        };
    }
}

/// <summary>
/// Servicio Mock de Disponibilidad para testing
/// </summary>
public class MockDisponibilidadService : IDisponibilidadService
{
    public async Task<List<DisponibilidadResponseDto>> ObtenerDisponibilidadesAsync()
    {
        await Task.Delay(500);
        return new List<DisponibilidadResponseDto>
        {
            new DisponibilidadResponseDto
            {
                Id = 1,
                DiaSemana = 1,
                DiaNombre = "Lunes",
                HoraInicio = new TimeSpan(8, 0, 0),
                HoraFin = new TimeSpan(17, 0, 0),
                DuracionMinutos = 30,
                Activo = true,
                CitasAsignadas = 8
            },
            new DisponibilidadResponseDto
            {
                Id = 2,
                DiaSemana = 2,
                DiaNombre = "Martes",
                HoraInicio = new TimeSpan(8, 0, 0),
                HoraFin = new TimeSpan(17, 0, 0),
                DuracionMinutos = 30,
                Activo = true,
                CitasAsignadas = 5
            }
        };
    }

    public async Task<DisponibilidadResponseDto> CrearDisponibilidadAsync(CrearDisponibilidadRequest request)
    {
        await Task.Delay(300);
        return new DisponibilidadResponseDto
        {
            Id = new Random().Next(1000),
            DiaSemana = request.DiaSemana,
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            DuracionMinutos = request.DuracionMinutos,
            Activo = request.Activo
        };
    }

    public async Task<DisponibilidadResponseDto> ActualizarDisponibilidadAsync(ActualizarDisponibilidadRequest request)
    {
        await Task.Delay(300);
        return new DisponibilidadResponseDto
        {
            Id = request.DisponibilidadId,
            Activo = request.Activo ?? true
        };
    }

    public async Task EliminarDisponibilidadAsync(int disponibilidadId)
    {
        await Task.Delay(300);
    }

    public async Task<DisponibilidadResponseDto?> ObtenerDisponibilidadPorIdAsync(int disponibilidadId)
    {
        await Task.Delay(300);
        return new DisponibilidadResponseDto { Id = disponibilidadId };
    }
}

/// <summary>
/// Servicio Mock de Autenticación para testing
/// </summary>
public class MockAuthService : IAuthService
{
    public async Task<AuthTokenResponse> LoginAsync(string usuario, string contrasena)
    {
        await Task.Delay(500);

        // Simular credenciales de prueba
        if (usuario == "doctor" && contrasena == "password")
        {
            return new AuthTokenResponse
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkb2N0b3IiLCJpYXQiOjE2OTQ2MjYwMDB9.mock_token",
                TokenType = "Bearer",
                ExpiresIn = 3600,
                RefreshToken = "refresh_mock_token"
            };
        }

        throw new UnauthorizedException("Credenciales inválidas");
    }

    public async Task LogoutAsync()
    {
        await Task.Delay(100);
    }

    public async Task<bool> EstaAutenticadoAsync()
    {
        await Task.Delay(100);
        return true;
    }
}

/// <summary>
/// Servicio Mock de Doctor para testing
/// </summary>
public class MockDoctorService : IDoctorService
{
    private int _nextId = 100;
    private int? _doctorIdCacheado;

    public async Task<DoctorResponseDto> RegistrarDoctorAsync(DoctorRequestDto datos)
    {
        await Task.Delay(500);

        var doctorId = _nextId++;
        _doctorIdCacheado = doctorId;

        return new DoctorResponseDto
        {
            Id = doctorId,
            Nombre = datos.Nombre,
            Apellido = datos.Apellido,
            Especialidad = datos.Especialidad,
            Email = datos.Email,
            Telefono = datos.Telefono,
            Consultorio = $"{100 + (doctorId % 10)}",
            FechaRegistro = DateTime.Now
        };
    }

    public async Task<DoctorResponseDto> ObtenerDoctorActualAsync()
    {
        await Task.Delay(300);

        if (!_doctorIdCacheado.HasValue)
            throw new UnauthorizedException("No hay doctor registrado");

        return new DoctorResponseDto
        {
            Id = _doctorIdCacheado.Value,
            Nombre = "Juan",
            Apellido = "Pérez",
            Especialidad = "Cardiología",
            Email = "juan@hospital.com",
            Consultorio = "201",
            FechaRegistro = DateTime.Now.AddMonths(-1)
        };
    }

    public async Task<DoctorResponseDto> ActualizarDoctorAsync(int doctorId, DoctorRequestDto datos)
    {
        await Task.Delay(300);

        return new DoctorResponseDto
        {
            Id = doctorId,
            Nombre = datos.Nombre,
            Apellido = datos.Apellido,
            Especialidad = datos.Especialidad,
            Email = datos.Email,
            Telefono = datos.Telefono,
            FechaRegistro = DateTime.Now
        };
    }

    public async Task<List<CitaResponseDto>> GetCitasByDoctorIdAsync(int doctorId)
    {
        await Task.Delay(500);

        // Retornar lista mock de citas para el doctor
        return new List<CitaResponseDto>
        {
            new CitaResponseDto
            {
                Id = 1,
                PacienteId = 101,
                PacienteNombre = "Juan Pérez",
                PacienteCedula = "1234567890",
                FechaHora = DateTime.Now.AddDays(1),
                Motivo = "Consulta general",
                Confirmada = true,
                Asistio = false,
                DuracionMinutos = 30,
                Estado = "Confirmada"
            },
            new CitaResponseDto
            {
                Id = 2,
                PacienteId = 102,
                PacienteNombre = "María García",
                PacienteCedula = "0987654321",
                FechaHora = DateTime.Now.AddDays(2),
                Motivo = "Seguimiento",
                Confirmada = false,
                Asistio = false,
                DuracionMinutos = 30,
                Estado = "Pendiente"
            }
        };
    }

    public void EstablecerDoctorId(int doctorId)
    {
        _doctorIdCacheado = doctorId;
    }

    public int? ObtenerDoctorIdCacheado()
    {
        return _doctorIdCacheado;
    }
}
