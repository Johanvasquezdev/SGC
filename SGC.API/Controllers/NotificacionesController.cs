using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly IPrefNotificacionService _prefService;

        public NotificacionesController(
            INotificacionService notificacionService,
            IPrefNotificacionService prefService)
        {
            _notificacionService = notificacionService;
            _prefService = prefService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var notificacion = await _notificacionService
                .GetByIdAsync(id);
            return Ok(notificacion);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var notificaciones = await _notificacionService
                .GetByUsuarioAsync(usuarioId);
            return Ok(notificaciones);
        }

        [HttpGet("usuario/{usuarioId}/no-leidas")]
        public async Task<IActionResult> GetNoLeidas(int usuarioId)
        {
            var notificaciones = await _notificacionService
                .GetNoLeidasAsync(usuarioId);
            return Ok(notificaciones);
        }

        [HttpPut("{id}/leer")]
        public async Task<IActionResult> MarcarLeida(int id)
        {
            await _notificacionService.MarcarLeidaAsync(id);
            return NoContent();
        }

        [HttpGet("preferencias/{usuarioId}")]
        public async Task<IActionResult> GetPreferencias(int usuarioId)
        {
            var preferencias = await _prefService
                .GetByUsuarioAsync(usuarioId);
            return Ok(preferencias);
        }

        [HttpPost("preferencias")]
        public async Task<IActionResult> GuardarPreferencias(
            [FromBody] Application.DTOs.Notifications.PrefNotificacionRequest request)
        {
            var preferencias = await _prefService.GuardarAsync(request);
            return Ok(preferencias);
        }
    }
}
