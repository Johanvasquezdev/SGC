using System;
using SGC.Domain.Base;

namespace SGC.Domain.Entities.Appointments
{
    // Clase que representa la disponibilidad de un médico para citas hereda de EntidadBase para incluir propiedades comunes como Id y FechaCreacion
    public class Disponibilidad : EntidadBase
    {
        public int MedicoId { get; set; } // Relación con Médico
        public string DiaSemana { get; set; } // Lunes, Martes, Miércoles, Jueves, Viernes, Sábado, Domingo
        public TimeSpan HoraInicio { get; set; }  // Hora de inicio de la disponibilidad
        public TimeSpan HoraFin { get; set; } // Hora de fin de la disponibilidad
    }
}