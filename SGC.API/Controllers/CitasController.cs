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
        // Controlador para la gestion de citas medicas, con acciones para pacientes y medicos segun su rol. Se utiliza el servicio de citas para la logica de negocio.
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet("{id}")]
        // GET api/citas/{id} - Obtiene una cita por su ID, accesible para pacientes y medicos (si es su cita)
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citaService.GetByIdAsync(id);
            return Ok(cita);
        }

        [HttpGet("paciente/{id}")]
        // GET api/citas/paciente/{id} - Obtiene las citas de un paciente, solo para el paciente autenticado
        public async Task<IActionResult> GetByPaciente(int id)
        {
            var citas = await _citaService.GetByPacienteAsync(id);
            return Ok(citas);
        }

        [HttpGet("medico/agenda")]
        // GET api/citas/medico/agenda?fecha=2024-06-01 - Obtiene las citas de un medico para una fecha especifica, solo para el medico autenticado
        public async Task<IActionResult> GetAgendaMedico(
            [FromQuery] DateTime fecha)
        {
            var medicoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var citas = await _citaService.GetByMedicoAsync(medicoId);
            var citasFecha = citas.Where(
                c => c.FechaHora.Date == fecha.Date);
            return Ok(citasFecha);
        }

        [HttpGet("medico")]
        [Authorize(Roles = "Medico,Administrador")]
        // GET api/citas/medico - Obtiene todas las citas del medico autenticado
        public async Task<IActionResult> GetCitasMedico()
        {
            var medicoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var citas = await _citaService.GetByMedicoAsync(medicoId);
            return Ok(citas);
        }

        [HttpPost]
        // POST api/citas - Crea una nueva cita, solo para pacientes
        public async Task<IActionResult> Crear(
            [FromBody] CrearCitaRequest request)
        {
            var cita = await _citaService.AgendarAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = cita.Id }, cita);
        }

        [HttpPut("{id}/confirmar")]
        [Authorize(Roles = "Medico,Administrador")]
        // PUT api/citas/{id}/confirmar - Confirma una cita, solo para medicos y administradores
        public async Task<IActionResult> Confirmar(int id)
        {
            await _citaService.ConfirmarAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/cancelar")]
        // PUT api/citas/{id}/cancelar - Cancela una cita, para pacientes (si es su cita) y medicos (si es su cita o por cualquier motivo)
        public async Task<IActionResult> Cancelar(int id,
            [FromBody] ActualizarCitaRequest request)
        {
            await _citaService.CancelarAsync(id,
                request.Motivo ?? "Sin motivo especificado");
            return NoContent();
        }

        [HttpPut("{id}/rechazar")]
        [Authorize(Roles = "Medico")]
        // PUT api/citas/{id}/rechazar - Rechaza una cita, solo para medicos (si es su cita)
        public async Task<IActionResult> Rechazar(int id,
            [FromBody] ActualizarCitaRequest request)
        {
            await _citaService.RechazarAsync(id,
                request.Motivo ?? "Sin motivo especificado");
            return NoContent();
        }

        [HttpPut("{id}/reprogramar")]
        // PUT api/citas/{id}/reprogramar - Reprograma una cita, para pacientes (si es su cita) y medicos (si es su cita o por cualquier motivo)
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
        // PUT api/citas/{id}/iniciar-consulta - Inicia la consulta de una cita, solo para medicos (si es su cita)
        public async Task<IActionResult> IniciarConsulta(int id)
        {
            await _citaService.IniciarConsultaAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/asistencia")]
        [Authorize(Roles = "Medico")]
        // PUT api/citas/{id}/asistencia?asistio=true - Marca la asistencia de una cita, solo para medicos (si es su cita)
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
