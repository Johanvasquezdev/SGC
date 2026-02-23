using SGC.Domain.Entities.Security;

namespace SGC.Domain.Entities.Medical
{
    public class Paciente : Usuario // El paciente es un tipo de usuario 
    {
        public string Cedula { get; set; } 
        public string Telefono { get; set; } 
        public string Direccion { get; set; } 
        public string TipoSeguro { get; set; } 
    }
}