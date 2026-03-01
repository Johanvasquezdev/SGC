using SGC.Domain.Entities.Security;

namespace SGC.Domain.Entities.Medical
{
    //  la clase Paciente representa a un paciente del sistema, con propiedades para cédula, teléfono, fecha de nacimiento, tipo de seguro y número de seguro. Incluye un método para calcular la edad del paciente basado en su fecha de nacimiento.
    public sealed class Paciente : Usuario
    {
        public string? Cedula { get; set; }
        public string? Telefono { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? TipoSeguro { get; set; }
        public string? NumeroSeguro { get; set; }

      
        // calcula la edad del paciente. Retorna null si no se ha registrado la fecha de nacimiento.
        public int? CalcularEdad()
        {
            if (FechaNacimiento == null) return null;

            var hoy = DateOnly.FromDateTime(DateTime.UtcNow);

            var edad = hoy.Year - FechaNacimiento.Value.Year;

            if (FechaNacimiento.Value > 
                hoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}