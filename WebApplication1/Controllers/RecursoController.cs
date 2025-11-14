using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;

namespace Onboarding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController : ControllerBase
    {
        private readonly IRecursoService _recursoService;

        public RecursoController(IRecursoService recursoService)
        {
            _recursoService = recursoService;
        }

        // 🔹 GET: api/Recurso
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var recursos = await _recursoService.GetAllAsync();
            return Ok(recursos);
        }

        // 🔹 GET: api/Recurso/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var recurso = await _recursoService.GetByIdAsync(id);
            if (recurso == null)
                return NotFound(new { message = "Recurso no encontrado." });

            return Ok(recurso);
        }

        // 🔹 POST: api/Recurso
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecursoCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _recursoService.CreateAsync(dto);
            return Ok(new { message = "Recurso creado exitosamente." });
        }

        // 🔹 PUT: api/Recurso/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RecursoCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _recursoService.UpdateAsync(id, dto);
            return Ok(new { message = "Recurso actualizado correctamente." });
        }

        // 🔹 DELETE: api/Recurso/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _recursoService.DeleteAsync(id);
            return Ok(new { message = "Recurso eliminado correctamente." });
        }
    }
}
