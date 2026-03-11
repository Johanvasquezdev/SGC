using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGC.Application.Contracts;
using SGC.Application.DTOs.Appointments;

namespace SGC.API.Controllers
{
    // Controlador para la gestion de citas medicas
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

        // GET api/citas/paciente/{id} - Obtiene todas las citas de un paciente
        [HttpGet("paciente/{id}")]
        public async Task<IActionResult> GetByPaciente(int id)
        {
            var citas = await _citaService.GetByPacienteAsync(id);
            return Ok(citas);
        }

        // GET api/citas/medico/agenda?fecha={fecha} - Obtiene la agenda del medico para una fecha
        [HttpGet("medico/agenda")]
        public async Task<IActionResult> GetAgendaMedico([FromQuery] DateTime fecha)
        {
            // Obtener el ID del medico autenticado desde el token
            var medicoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var citas = await _citaService.GetByMedicoAsync(medicoId);

            // Filtrar por la fecha solicitada
            var citasFecha = citas.Where(c => c.FechaHora.Date == fecha.Date);
            return Ok(citasFecha);
        }

        // POST api/citas - Agenda una nueva cita medica
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCitaRequest request)
        {
            var cita = await _citaService.AgendarAsync(request);
            return CreatedAtAction(nameof(GetByPaciente), new { id = cita.PacienteId }, cita);
        }

        // PUT api/citas/{id}/reprogramar - Reprograma una cita a nueva fecha
        [HttpPut("{id}/reprogramar")]
        public async Task<IActionResult> Reprogramar(int id, [FromBody] ActualizarCitaRequest request)
        {
            if (request.FechaHora == null)
                return BadRequest(new { mensaje = "La nueva fecha es requerida para reprogramar." });

            await _citaService.ReprogramarAsync(id, request.FechaHora.Value);
            return NoContent();
        }

        // PUT api/citas/{id}/cancelar - Cancela una cita con motivo
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id, [FromBody] ActualizarCitaRequest request)
        {
            await _citaService.CancelarAsync(id, request.Motivo ?? "Sin motivo especificado");
            return NoContent();
        }

        // PUT api/citas/{id}/asistencia - Marca asistencia o no asistencia (solo medico)
        [HttpPut("{id}/asistencia")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> MarcarAsistencia(int id, [FromQuery] bool asistio)
        {
            if (asistio)
                await _citaService.CompletarAsync(id);
            else
                await _citaService.MarcarNoAsistioAsync(id);

            return NoContent();
        }
    }
}
