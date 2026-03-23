using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using SGC.Domain.Interfaces;

namespace SGC.Infraestructure.Email
{
    // Servicio de correo electr�nico para enviar notificaciones a los pacientes
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        // M�todo privado para enviar un correo electr�nico gen�rico
        private async Task EnviarAsync(string destinatario,
            string asunto, string cuerpo)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(
                _config["Email:From"]));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = asunto;
            mensaje.Body = new TextPart("html") { Text = cuerpo };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["Email:Host"],
                int.Parse(_config["Email:Port"]!),
                SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _config["Email:Username"],
                _config["Email:Password"]);
            await smtp.SendAsync(mensaje);
            await smtp.DisconnectAsync(true);
        }

        // M�todos para enviar confirmaciones, recordatorios de cita y cancelaciones de cita a los pacientes
        public async Task EnviarConfirmacionCitaAsync(string email,
            string nombrePaciente, DateTime fechaCita)
        {
            var asunto = "Confirmaci�n de Cita � MedAgenda";
            var cuerpo = $@"
                <h2>Hola {nombrePaciente}</h2>
                <p>Tu cita ha sido confirmada para el 
                   <strong>{fechaCita:dd/MM/yyyy HH:mm}</strong></p>
                <p>Gracias por usar MedAgenda.</p>";
            await EnviarAsync(email, asunto, cuerpo);
        }

      
        public async Task EnviarRecordatorioCitaAsync(string email,
            string nombrePaciente, DateTime fechaCita)
        {
            var asunto = "Recordatorio de Cita � MedAgenda";
            var cuerpo = $@"
                <h2>Hola {nombrePaciente}</h2>
                <p>Te recordamos que tienes una cita ma�ana 
                   <strong>{fechaCita:dd/MM/yyyy HH:mm}</strong></p>
                <p>Gracias por usar MedAgenda.</p>";
            await EnviarAsync(email, asunto, cuerpo);
        }

        public async Task EnviarCancelacionCitaAsync(string email,
            string nombrePaciente, DateTime fechaCita)
        {
            var asunto = "Cancelaci�n de Cita � MedAgenda";
            var cuerpo = $@"
                <h2>Hola {nombrePaciente}</h2>
                <p>Tu cita del 
                   <strong>{fechaCita:dd/MM/yyyy HH:mm}</strong> 
                   ha sido cancelada.</p>
                <p>Gracias por usar MedAgenda.</p>";
            await EnviarAsync(email, asunto, cuerpo);
        }
    }
}
