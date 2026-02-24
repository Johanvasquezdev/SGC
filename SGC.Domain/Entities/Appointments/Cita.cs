using SGC.Domain.Base;
using SGC.Domain.Enums;
using SGC.Domain.Exceptions;

namespace SGC.Domain.Entities.Appointments
{
    // la clase cita representa una cita médica entre un paciente y un médico, con propiedades para la fecha y hora, estado de la cita, motivo de cancelación o reprogramación, y notas adicionales. Incluye reglas de negocio para confirmar, cancelar, reprogramar, marcar como no asistió o completar la cita, asegurando que las transiciones de estado sean coherentes con el flujo típico de una cita médica.
    public class Cita : EntidadBase
    {
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int? DisponibilidadId { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoCita Estado { get; set; } = EstadoCita.Solicitada;
        public string? Motivo { get; set; }
        public string? Notas { get; set; }

        // Constructor para crear una nueva cita
        public void Confirmar()
        {
            if (Estado != EstadoCita.Solicitada)
                throw new CitaConflictoException(
                    "Solo se puede confirmar una cita en estado Solicitada.");
            Estado = EstadoCita.Confirmada;
        }

        // El método Cancelar permite cancelar una cita, pero solo si no está completada. Si la cita ya está completada, se lanza una excepción CitaConflictoException. Al cancelar, se establece el estado de la cita a Cancelada y se registra el motivo de la cancelación.
        public void Cancelar(string motivo)
        {
            if (Estado == EstadoCita.Completada)
                throw new CitaConflictoException(
                    "No se puede cancelar una cita completada.");
            Estado = EstadoCita.Cancelada;
            Motivo = motivo;
        }

        // metodo Reprogramar permite cambiar la fecha y hora de una cita, pero solo si la cita no está completada ni cancelada. Si la cita ya está en uno de esos estados, se lanza una excepción CitaConflictoException. Al reprogramar, se actualiza la fecha y hora de la cita y se restablece su estado a Solicitada.
        public void Reprogramar(DateTime nuevaFecha)
        {
            if (Estado == EstadoCita.Completada || Estado == EstadoCita.Cancelada)
                throw new CitaConflictoException(
                    "No se puede reprogramar una cita completada o cancelada.");
            FechaHora = nuevaFecha;
            Estado = EstadoCita.Solicitada;
        }

        // metodo para marcar una cita como NoAsistio, pero solo si la cita está en estado Confirmada. Si la cita no está confirmada, se lanza una excepción CitaConflictoException. Al marcar como NoAsistio, se actualiza el estado de la cita a NoAsistio.
        public void MarcarNoAsistio()
        {
            if (Estado != EstadoCita.Confirmada)
                throw new CitaConflictoException(
                    "Solo se puede marcar NoAsistio en una cita Confirmada.");
            Estado = EstadoCita.NoAsistio;
        }

        // metodo para completar una cita, solo si la cita no esta en progreso
        public void Completar()
        {
            if (Estado != EstadoCita.EnProgreso)
                throw new CitaConflictoException(
                    "Solo se puede completar una cita En Progreso.");
            Estado = EstadoCita.Completada;
        }
    }
}