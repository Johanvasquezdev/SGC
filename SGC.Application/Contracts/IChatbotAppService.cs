using SGC.Application.DTOs.Chat;
using System.Threading.Tasks;

namespace SGC.Application.Contracts
{
    public interface IChatbotAppService
    {
        Task<ChatResponse> EnviarMensajeAsync(ChatRequest request);
    }
}