using SGC.Application.Contracts;
using SGC.Application.DTOs.Chat;
using SGC.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class ChatbotAppService : IChatbotAppService
    {
        private readonly IChatbotService _chatbotService;
        private readonly ISGCLogger _logger;

        public ChatbotAppService(IChatbotService chatbotService,
            ISGCLogger logger)
        {
            _chatbotService = chatbotService;
            _logger = logger;
        }

        public async Task<ChatResponse> EnviarMensajeAsync(
            ChatRequest request)
        {
            _logger.LogInfo(
                $"Chatbot recibe mensaje de usuario {request.UsuarioId}");

            var respuesta = await _chatbotService
                .EnviarMensajeAsync(request.Mensaje, request.Contexto);

            return new ChatResponse
            {
                Respuesta = respuesta,
                FechaRespuesta = DateTime.UtcNow
            };
        }
    }
}