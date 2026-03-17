using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Validators
{
    // Valida los datos de un paciente antes de crear o actualizar su registro
    public class PacienteValidator
    {
        // Valida que el nombre, email y fecha de nacimiento del paciente sean correctos
        public void Validar(Paciente paciente)
        {
            if (string.IsNullOrWhiteSpace(paciente.Nombre))
                throw new InvalidOperationException(
                    "El nombre del paciente es requerido.");

            if (string.IsNullOrWhiteSpace(paciente.Email))
                throw new InvalidOperationException(
                    "El email del paciente es requerido.");

            if (!paciente.Email.Contains("@"))
                throw new InvalidOperationException(
                    "El email no tiene un formato válido.");

            if (paciente.FechaNacimiento.HasValue &&
                paciente.FechaNacimiento.Value >
                DateOnly.FromDateTime(DateTime.UtcNow))
                throw new InvalidOperationException(
                    "La fecha de nacimiento no puede ser en el futuro.");
        }
    }
}