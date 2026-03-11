using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;

namespace SGC.API.Controllers
{
    // Controlador para consultar informacion de medicos
    [Route("api/medicos")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET api/medicos?especialidadId={id} - Obtiene medicos, opcionalmente filtrados por especialidad
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? especialidadId)
        {
            if (especialidadId.HasValue)
            {
                var medicosPorEsp = await _medicoService.GetByEspecialidadAsync(especialidadId.Value);
                return Ok(medicosPorEsp);
            }

            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        // GET api/medicos/{id} - Obtiene un medico por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            return Ok(medico);
        }
    }
}
