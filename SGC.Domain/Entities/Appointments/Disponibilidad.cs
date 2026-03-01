using SGC.Domain.Base;
using SGC.Domain.Enums;

namespace SGC.Domain.Entities.Appointments
{
    // La clase Disponibilidad representa los horarios disponibles de un médico para atender citas. Incluye propiedades para el día de la semana, horas de inicio y fin, duración de las citas, y si es recurrente. También tiene una regla de validación para asegurar que el horario sea coherente.
    public class Disponibilidad : EntidadBase
    {
        public int MedicoId { get; set; }
        public DiaSemana DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int DuracionCitaMin { get; set; }
        public bool EsRecurrente { get; set; } = true;


        // valida que el horario sea coherente
        public bool EsValido()
        {
            return HoraFin > HoraInicio && DuracionCitaMin > 0;
        }
    }
}