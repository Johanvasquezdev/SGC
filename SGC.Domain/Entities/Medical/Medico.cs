using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Security;
using SGC.Domain.Exceptions;

namespace SGC.Domain.Entities.Medical
{
    public sealed class Medico : Usuario // el médico hereda de usuario porque también es un usuario del sistema, pero con atributos y comportamientos específicos relacionados con su rol como médico
    {
        public string Exequatur { get; set; } = string.Empty; // el número de exequatur es un identificador único que se le asigna a cada médico para certificar que está autorizado para ejercer la medicina, por lo que es un atributo esencial para la entidad Medico
        public int? EspecialidadId { get; set; }
        public int? ProveedorSaludId { get; set; }
        public string TelefonoConsultorio { get; set; } = string.Empty;
        public string? Foto { get; set; }
        public List<Disponibilidad> Horarios { get; set; } = new();



        // verifica si el médico tiene disponibilidad
        public bool TieneDisponibilidad(DateTime fechaHora)
        {
            return Horarios.Any(h =>
                h.DiaSemana == fechaHora.DayOfWeek.ToString() &&
                h.HoraInicio <= fechaHora.TimeOfDay &&
                h.HoraFin >= fechaHora.TimeOfDay &&
                h.EsValido());
        }


        // agrega un horario validando que no se solape
        public void AgregarHorario(Disponibilidad nuevaDisponibilidad)
        {
            if (!nuevaDisponibilidad.EsValido())
                throw new HorarioNoDisponibleException(Id, DateTime.UtcNow);

            bool seSolapa = Horarios.Any(h =>
                h.DiaSemana == nuevaDisponibilidad.DiaSemana &&
                h.HoraInicio < nuevaDisponibilidad.HoraFin &&
                h.HoraFin > nuevaDisponibilidad.HoraInicio);

            if (seSolapa)
                throw new CitaConflictoException("El horario se solapa con uno existente.");

            Horarios.Add(nuevaDisponibilidad);
        }
    }
}