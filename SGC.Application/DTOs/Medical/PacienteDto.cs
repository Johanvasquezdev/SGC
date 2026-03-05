namespace SGC.Application.DTOs.Medical
{
    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string TipoSeguro { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
