namespace DoctorApp.DTOs.Responses;

/// <summary>
/// DTO para la respuesta de agenda
/// </summary>
public class ObtenerAgendaResponse
{
    public DateTime Fecha { get; set; }
    public List<CitaResponseDto> Citas { get; set; } = new();
    public int TotalCitas { get; set; }
    public int CitasConfirmadas { get; set; }
    public int CitasPendientes { get; set; }
}

/// <summary>
/// DTO para una cita en la respuesta
/// </summary>
public class CitaResponseDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int PacienteId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteCedula { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public bool Confirmada { get; set; }
    public bool Asistio { get; set; }
    public int DuracionMinutos { get; set; }
    public string Estado { get; set; } = string.Empty;
}

/// <summary>
/// DTO para disponibilidad
/// </summary>
public class DisponibilidadResponseDto
{
    public int Id { get; set; }
    public int DiaSemana { get; set; }
    public string DiaNombre { get; set; } = string.Empty;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public int DuracionMinutos { get; set; }
    public bool Activo { get; set; }
    public int CitasAsignadas { get; set; }
}

/// <summary>
/// DTO para respuesta de token
/// </summary>
public class AuthTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// DTO para respuesta genérica de error
/// </summary>
public class ApiErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}
