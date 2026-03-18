using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using System.Threading.Tasks;

namespace SGC.API.Controllers
{
    [Route("api/auditoria")]
    [ApiController]
    // [Authorize(Roles = "Administrador")]
    public class AuditoriasController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriasController(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? entidad,
            [FromQuery] int? usuarioId)
        {
            if (!string.IsNullOrEmpty(entidad))
            {
                var porEntidad = await _auditoriaService
                    .GetByEntidadAsync(entidad);
                return Ok(porEntidad);
            }

            if (usuarioId.HasValue)
            {
                var porUsuario = await _auditoriaService
                    .GetByUsuarioAsync(usuarioId.Value);
                return Ok(porUsuario);
            }

            return BadRequest("Debe especificar entidad o usuarioId.");
        }
    }
}
