using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;
using System.Security.Claims;

namespace SGC.API.Controllers
{
    [Route("api/citas")]
    [ApiController]
    [Authorize]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            return Ok(cita);
        }

        [HttpGet("paciente/{id}")]
        public async Task<IActionResult> GetByPaciente(int id)
        {
            var citas = await _citaService.GetByPacienteAsync(id);
            return Ok(citas);
        }

        [HttpGet("medico/agenda")]
        public async Task<IActionResult> GetAgendaMedico(
            [FromQuery] DateTime fecha)
        {
            var medicoId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var citas = await _citaService.GetByMedicoAsync(medicoId);
            var citasFecha = citas.Where(
                c => c.FechaHora.Date == fecha.Date);
            return Ok(citasFecha);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearCitaRequest request)
        {
            var cita = await _citaService.AgendarAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = cita.Id }, cita);
        }

        [HttpPut("{id}/confirmar")]
        [Authorize(Roles = "Medico,Administrador")]
        public async Task<IActionResult> Confirmar(int id)
        {
            await _citaService.ConfirmarAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id,
            [FromBody] ActualizarCitaRequest request)
        {
            await _citaService.CancelarAsync(id,
                request.Motivo ?? "Sin motivo especificado");
            return NoContent();
        }

        [HttpPut("{id}/rechazar")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Rechazar(int id,
            [FromBody] ActualizarCitaRequest request)
        {
            await _citaService.RechazarAsync(id,
                request.Motivo ?? "Sin motivo especificado");
            return NoContent();
        }

        [HttpPut("{id}/reprogramar")]
        public async Task<IActionResult> Reprogramar(int id,
            [FromBody] ActualizarCitaRequest request)
        {
            if (request.FechaHora == null)
                return BadRequest(
                    new { mensaje = "La nueva fecha es requerida." });
            await _citaService.ReprogramarAsync(id, request.FechaHora.Value);
            return NoContent();
        }

        [HttpPut("{id}/iniciar-consulta")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> IniciarConsulta(int id)
        {
            await _citaService.IniciarConsultaAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/asistencia")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> MarcarAsistencia(int id,
            [FromQuery] bool asistio)
        {
            if (asistio)
                await _citaService.CompletarAsync(id);
            else
                await _citaService.MarcarNoAsistioAsync(id);
            return NoContent();
        }
    }
}