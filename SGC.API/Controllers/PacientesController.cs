using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Medical;

namespace SGC.API.Controllers
{
    // Controlador para la gestion de pacientes
    [Route("api/pacientes")]
    [ApiController]
    [Authorize]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET api/pacientes - Obtiene todos los pacientes (admin o medico)
        [HttpGet]
        [Authorize(Roles = "Administrador,Medico")]
        public async Task<IActionResult> GetAll()
        {
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        // GET api/pacientes/{id} - Obtiene un paciente por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            return Ok(paciente);
        }

        // GET api/pacientes/cedula/{cedula} - Busca un paciente por cedula
        [HttpGet("cedula/{cedula}")]
        public async Task<IActionResult> GetByCedula(string cedula)
        {
            var paciente = await _pacienteService.GetByCedulaAsync(cedula);
            return Ok(paciente);
        }

        // POST api/pacientes - Registra un nuevo paciente
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Crear([FromBody] CrearPacienteRequest request)
        {
            var paciente = await _pacienteService.CrearAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        // PUT api/pacientes/{id} - Actualiza la informacion de un paciente
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarPacienteRequest request)
        {
            request.Id = id;
            await _pacienteService.ActualizarAsync(request);
            return NoContent();
        }

        // PUT api/pacientes/{id}/desactivar - Desactiva un paciente
        [HttpPut("{id}/desactivar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _pacienteService.DesactivarAsync(id);
            return NoContent();
        }

        // PUT api/pacientes/{id}/activar - Activa un paciente
        [HttpPut("{id}/activar")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Activar(int id)
        {
            await _pacienteService.ActivarAsync(id);
            return NoContent();
        }
    }
}
