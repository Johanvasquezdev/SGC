using System;
using SGC.Domain.Base;

namespace SGC.Domain.Entities.Appointments
{
    public class Disponibilidad : EntidadBase
    {
        public int MedicoId { get; set; } 
        public string DiaSemana { get; set; } 
        public TimeSpan HoraInicio { get; set; } 
        public TimeSpan HoraFin { get; set; } 
    }
}