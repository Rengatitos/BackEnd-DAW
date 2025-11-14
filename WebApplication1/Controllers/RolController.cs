using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;

namespace Onboarding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // 🔹 GET: api/Rol
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolService.GetAllAsync();
            return Ok(roles);
        }

        // 🔹 GET: api/Rol/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var rol = await _rolService.GetByIdAsync(id);
            if (rol == null)
                return NotFound(new { message = "Rol no encontrado." });

            return Ok(rol);
        }

        // 🔹 POST: api/Rol
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolCreateDTO rolDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newRol = await _rolService.CreateAsync(rolDto);
            return CreatedAtAction(nameof(GetById), new { id = newRol.Id }, newRol);
        }

        // 🔹 PUT: api/Rol/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RolCreateDTO rolDto)
        {
            var updated = await _rolService.UpdateAsync(id, rolDto);
            if (!updated)
                return NotFound(new { message = "No se pudo actualizar, rol no encontrado." });

            return NoContent();
        }

        // 🔹 DELETE: api/Rol/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _rolService.DeleteAsync(id);
            return NoContent();
        }
    }
}
