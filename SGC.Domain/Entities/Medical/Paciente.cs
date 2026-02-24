using SGC.Domain.Entities.Security;

namespace SGC.Domain.Entities.Medical
{
    //  la clase Paciente representa a un paciente del sistema, con propiedades para cédula, teléfono, fecha de nacimiento, tipo de seguro y número de seguro. Incluye un método para calcular la edad del paciente basado en su fecha de nacimiento.
    public sealed class Paciente : Usuario
    {
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public string TipoSeguro { get; set; } = string.Empty;
        public string NumeroSeguro { get; set; } = string.Empty;

      
        // calcula la edad del paciente
        public int CalcularEdad()
        {
            var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

            var edad = hoy.Year - FechaNacimiento.Year;

            if (FechaNacimiento > 
                hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}