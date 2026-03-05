using Microsoft.AspNetCore.Mvc;
using SGC.Application.DTOs.Notifications;
using SGC.Application.Services.Notifications;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrefNotificacionesController : ControllerBase
    {
        private readonly IPrefNotificacionService _service;

        public PrefNotificacionesController(IPrefNotificacionService service)
        {
            _service = service;
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var result = await _service.GetByUsuarioIdAsync(usuarioId);
            return Ok(result);
        }

        [HttpPost("usuario/{usuarioId:int}")]
        public async Task<IActionResult> Create(int usuarioId)
        {
            var result = await _service.CreateAsync(usuarioId);
            return CreatedAtAction(nameof(GetByUsuario), new { usuarioId = result.UsuarioId }, result);
        }

        [HttpPut("usuario/{usuarioId:int}")]
        public async Task<IActionResult> Update(int usuarioId, [FromBody] UpdatePrefNotificacionRequest request)
        {
            var result = await _service.UpdateAsync(usuarioId, request);
            return Ok(result);
        }
    }
}
