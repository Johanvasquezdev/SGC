using Microsoft.AspNetCore.Mvc;
using SGC.Application.DTOs.Medical;
using SGC.Application.Services.Medical;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _service;

        public MedicosController(IMedicoService service)
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

        [HttpGet("exequatur/{exequatur}")]
        public async Task<IActionResult> GetByExequatur(string exequatur)
        {
            var result = await _service.GetByExequaturAsync(exequatur);
            return Ok(result);
        }

        [HttpGet("especialidad/{especialidadId:int}")]
        public async Task<IActionResult> GetByEspecialidad(int especialidadId)
        {
            var result = await _service.GetByEspecialidadAsync(especialidadId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicoRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicoRequest request)
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
