using SGC.Application.Contracts;
using SGC.Application.DTOs.Chat;
using SGC.Application.Services.Base;
using SGC.Domain.Interfaces;
using SGC.Domain.Interfaces.ILogger;
using System;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class ChatbotAppService : BaseService, IChatbotAppService
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotAppService(
            IChatbotService chatbotService,
            ISGCLogger logger) : base(logger)
        {
            _chatbotService = chatbotService;
        }

        public async Task<ChatResponse> EnviarMensajeAsync(
            ChatRequest request)
        {
            LogOperacion("EnviarMensaje",
                $"UsuarioId: {request.UsuarioId}");

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