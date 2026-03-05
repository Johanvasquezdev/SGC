namespace SGC.Application.DTOs.Medical
{
    public class UpdatePacienteRequest
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string TipoSeguro { get; set; } = null!;
    }
}
