using Microsoft.AspNetCore.Mvc;
using SGC.Application.DTOs.Appointments;
using SGC.Application.Services.Appointments;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisponibilidadController : ControllerBase
    {
        private readonly IDisponibilidadService _service;

        public DisponibilidadController(IDisponibilidadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("medico/{medicoId:int}")]
        public async Task<IActionResult> GetByMedico(int medicoId)
        {
            var result = await _service.GetByMedicoIdAsync(medicoId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDisponibilidadRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDisponibilidadRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
