using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Validators
{
    // Validador para la entidad Medico. Se encarga de validar las reglas de negocio antes de crear o actualizar un medico.
    public class MedicoValidator
    {
        // Valida los datos de un medico. Lanza excepciones si alguna regla de negocio no se cumple.
        public void Validar(Medico medico)
        {
            if (string.IsNullOrWhiteSpace(medico.Nombre))
                throw new InvalidOperationException(
                    "El nombre del médico es requerido.");

            if (string.IsNullOrWhiteSpace(medico.Email))
                throw new InvalidOperationException(
                    "El email del médico es requerido.");

            if (!medico.Email.Contains("@"))
                throw new InvalidOperationException(
                    "El email no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(medico.Exequatur))
                throw new InvalidOperationException(
                    "El exequátur del médico es requerido.");

            if (medico.EspecialidadId == null)
                throw new InvalidOperationException(
                    "El médico debe tener una especialidad asignada.");
        }
    }
}