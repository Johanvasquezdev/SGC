namespace SGC.Application.DTOs.Medical
{
    public class CreateMedicoRequest
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Exequatur { get; set; } = null!;
        public int EspecialidadId { get; set; }
        public string TelefonoConsultorio { get; set; } = null!;
    }
}
