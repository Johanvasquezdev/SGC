using Microsoft.AspNetCore.Mvc;
using SGC.Application.Services.Notifications;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _service;

        public NotificacionesController(INotificacionService service)
        {
            _service = service;
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var result = await _service.GetByUsuarioIdAsync(usuarioId);
            return Ok(result);
        }

        [HttpGet("usuario/{usuarioId:int}/noleidas")]
        public async Task<IActionResult> GetNoLeidas(int usuarioId)
        {
            var result = await _service.GetNoLeidasAsync(usuarioId);
            return Ok(result);
        }

        [HttpPatch("{id:int}/leida")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            await _service.MarcarComoLeidaAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
