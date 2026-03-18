using SGC.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGC.Infraestructure.IA
{
    // Implementación mínima de chatbot para desarrollo y pruebas.
    public class AnthropicChatService : IChatbotService
    {
        // Responde con un mensaje fijo para pruebas.
        public Task<string> EnviarMensajeAsync(string mensaje, string? contexto = null)
        {
            return Task.FromResult($"[stub] Mensaje recibido: {mensaje}");
        }

        // Respuesta simulada de disponibilidad por médico y fecha.
        public Task<string> ConsultarDisponibilidadAsync(int medicoId, DateTime fecha)
        {
            return Task.FromResult($"[stub] Disponibilidad consultada para médico {medicoId} en {fecha:yyyy-MM-dd}.");
        }

        // Respuesta simulada con información de cita.
        public Task<string> ObtenerInfoCitaAsync(int citaId)
        {
            return Task.FromResult($"[stub] Información de la cita {citaId}.");
        }
    }
}
