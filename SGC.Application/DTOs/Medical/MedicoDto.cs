namespace SGC.Application.DTOs.Medical
{
    public class MedicoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Exequatur { get; set; } = null!;
        public int EspecialidadId { get; set; }
        public string TelefonoConsultorio { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
