using Microsoft.Extensions.Configuration;
using SGC.Domain.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace SGC.Infraestructure.IA
{
    // Servicio de chatbot utilizando la API de Anthropic para responder consultas de los pacientes sobre citas médicas
    public class AnthropicChatService : IChatbotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public AnthropicChatService(HttpClient httpClient,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["AI:ApiKey"]!;
            _model = config["AI:Model"]
                ?? "claude-sonnet-4-20250514";
        }

        //  Método privado para enviar la solicitud a la API de Anthropic y obtener la respuesta
        private async Task<string> EnviarRequestAsync(
            string mensaje, string sistema)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders
                .Add("x-api-key", _apiKey);
            _httpClient.DefaultRequestHeaders
                .Add("anthropic-version", "2023-06-01");

            var request = new
            {
                model = _model,
                max_tokens = 1000,
                system = sistema,
                messages = new[]
                {
                    new { role = "user", content = mensaje }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.anthropic.com/v1/messages", request);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AnthropicResponse>(
                json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result?.Content?[0]?.Text
                ?? "No pude procesar tu consulta.";
        }

        // Método público para enviar un mensaje al chatbot con un contexto opcional y obtener la respuesta generada por la IA
        public async Task<string> EnviarMensajeAsync(
            string mensaje, string? contexto = null)
        {
            var sistema = contexto ??
                "Eres un asistente médico de MedAgenda. " +
                "Ayudas a pacientes a agendar citas, consultar " +
                "disponibilidad y resolver dudas sobre el sistema.";

            return await EnviarRequestAsync(mensaje, sistema);
        }

        // Métodos específicos para consultas comunes de los pacientes sobre citas médicas, que formatean el mensaje con el contexto adecuado antes de enviarlo al chatbot
        public async Task<string> ConsultarDisponibilidadAsync(
            int medicoId, DateTime fecha)
        {
            var mensaje = $"El paciente consulta disponibilidad " +
                $"para el médico {medicoId} " +
                $"en la fecha {fecha:dd/MM/yyyy}";
            return await EnviarMensajeAsync(mensaje);
        }

        // Consulta sobre el estado de una cita específica, formateando el mensaje con el ID de la cita para que el chatbot pueda proporcionar la información relevante
        public async Task<string> ObtenerInfoCitaAsync(int citaId)
        {
            var mensaje = $"El paciente consulta información " +
                $"sobre su cita número {citaId}";
            return await EnviarMensajeAsync(mensaje);
        }

        // Consulta general sobre cómo usar el sistema, formateando el mensaje con la pregunta del paciente para que el chatbot pueda proporcionar una respuesta útil y orientativa
        private class AnthropicResponse
        {
            public List<ContentBlock>? Content { get; set; }
            public class ContentBlock
            {
                public string? Text { get; set; }
            }
        }
    }
}
