namespace SGC.Application.DTOs.Chat
{
    public class ChatRequest
    {
        public string Mensaje { get; set; } = string.Empty;
        public int? UsuarioId { get; set; }
        public string? Contexto { get; set; }
    }
}