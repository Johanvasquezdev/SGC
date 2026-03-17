using System;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface IEmailService // Interfaz para el servicio de correo electrónico que envía notificaciones a los pacientes sobre sus citas
    {
        Task EnviarConfirmacionCitaAsync(string email, string nombrePaciente, DateTime fechaCita);
        Task EnviarRecordatorioCitaAsync(string email, string nombrePaciente, DateTime fechaCita);
        Task EnviarCancelacionCitaAsync(string email, string nombrePaciente, DateTime fechaCita);
    }
}