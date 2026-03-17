using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Medical;

namespace SGC.API.Controllers
{
    [Route("api/medicos")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? especialidadId)
        {
            if (especialidadId.HasValue)
            {
                var porEsp = await _medicoService
                    .GetByEspecialidadAsync(especialidadId.Value);
                return Ok(porEsp);
            }
            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            return Ok(medico);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(
            [FromBody] CrearMedicoRequest request)
        {
            var medico = await _medicoService.CrearAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = medico.Id }, medico);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Medico")]
        public async Task<IActionResult> Actualizar(int id,
            [FromBody] ActualizarMedicoRequest request)
        {
            request.Id = id;
            await _medicoService.ActualizarAsync(request);
            return NoContent();
        }

        [HttpPut("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _medicoService.DesactivarAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar(int id)
        {
            await _medicoService.ActivarAsync(id);
            return NoContent();
        }
    }
}