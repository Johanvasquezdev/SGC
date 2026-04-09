namespace DoctorApp.DTOs.Responses;

/// <summary>
/// DTO para la respuesta al registrar un doctor
/// </summary>
public class DoctorResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Consultorio { get; set; }
    public DateTime FechaRegistro { get; set; }
}

/// <summary>
/// DTO para la respuesta de login de doctor
/// </summary>
public class DoctorLoginResponseDto
{
    public int DoctorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
}
