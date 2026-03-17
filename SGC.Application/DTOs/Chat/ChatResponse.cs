using System;

namespace SGC.Application.DTOs.Chat
{
    public class ChatResponse
    {
        public string Respuesta { get; set; } = string.Empty;
        public DateTime FechaRespuesta { get; set; } = DateTime.UtcNow;
    }
}