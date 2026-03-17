using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Domain.Interfaces.Repository;

namespace SGC.API.Controllers
{
    // Controlador para consultar registros de auditoria (solo administradores)
    [Route("api/auditoria")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaController(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        // GET api/auditoria - Obtiene todos los registros de auditoria
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? entidad,
            [FromQuery] int? usuarioId,
            [FromQuery] DateTime? fecha)
        {
            // Filtrar por entidad si se proporciona
            if (!string.IsNullOrEmpty(entidad))
            {
                var porEntidad = await _auditoriaRepository.GetByEntidadAsync(entidad);
                return Ok(porEntidad);
            }

            // Filtrar por usuario si se proporciona
            if (usuarioId.HasValue)
            {
                var porUsuario = await _auditoriaRepository.GetByUsuarioIdAsync(usuarioId.Value);
                return Ok(porUsuario);
            }

            // Filtrar por fecha si se proporciona
            if (fecha.HasValue)
            {
                var porFecha = await _auditoriaRepository.GetByFechaAsync(fecha.Value);
                return Ok(porFecha);
            }

            // Sin filtros, obtener todos
            var registros = await _auditoriaRepository.GetAllAsync();
            return Ok(registros);
        }
    }
}
