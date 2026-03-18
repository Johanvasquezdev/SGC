using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Chat;
using System.Threading.Tasks;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotAppService _chatbotService;

        public ChatbotController(IChatbotAppService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("mensaje")]
        public async Task<IActionResult> EnviarMensaje(
            [FromBody] ChatRequest request)
        {
            var respuesta = await _chatbotService
                .EnviarMensajeAsync(request);
            return Ok(respuesta);
        }
    }
}
