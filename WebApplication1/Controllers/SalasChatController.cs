using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using System.Threading.Tasks;
using SalaChatUpdateEstadoDTO = Onboarding.CORE.DTOs.SalaChatUpdateEstadoDTO;
using System.Collections.Generic;

namespace Onboarding.Api.Controllers
{
    [ApiController]
    [Route("api/salas")]
    public class SalasChatController : ControllerBase
    {
        private readonly ISalasChatService _service;

        public SalasChatController(ISalasChatService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var salas = await _service.GetAllAsync();
            return Ok(salas);
        }

        [HttpGet("{usuarioRef}")]
        public async Task<IActionResult> GetByUsuario(string usuarioRef)
        {
            var s = await _service.GetByUsuarioRefAsync(usuarioRef);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SalaChatCreateDTO dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetByUsuario), new { usuarioRef = created.UsuarioRef }, created);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{usuarioRef}/estado")]
        public async Task<IActionResult> UpdateEstado(string usuarioRef, [FromBody] SalaChatUpdateEstadoDTO dto)
        {
            var updated = await _service.UpdateEstadoAsync(usuarioRef, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPut("{usuarioRef}/contexto")]
        public async Task<IActionResult> UpdateContexto(string usuarioRef, [FromBody] SalaChatUpdateContextoDTO dto)
        {
            var updated = await _service.UpdateContextoAsync(usuarioRef, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{usuarioRef}")]
        public async Task<IActionResult> Delete(string usuarioRef)
        {
            var deleted = await _service.DeleteAsync(usuarioRef);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
