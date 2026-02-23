using System;
using SGC.Domain.Base;

namespace SGC.Domain.Entities.Appointments
{
    public class Cita : AuditEntity
    {
        public DateTime FechaHora { get; set; } // dia y hora de la cita
        public string Estado { get; set; } // Estados: Solicitada, Confirmada, Rechazada, Cancelada, Completada, NoAsistio 
        public string Motivo { get; set; } // Motivo de la Cita

        public int PacienteId { get; set; } // Relación con Paciente
        public int MedicoId { get; set; } // Relación con Médico
    }
}