using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;

namespace Onboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActividadController : ControllerBase
    {
        private readonly IActividadService _actividadService;

        public ActividadController(IActividadService actividadService)
        {
            _actividadService = actividadService;
        }

        // ✅ GET: api/actividad
        [HttpGet]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetAll()
        {
            var actividades = await _actividadService.GetAllAsync();
            return Ok(actividades);
        }

        // ✅ GET: api/actividad/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ActividadResponseDTO>> GetById(string id)
        {
            var actividad = await _actividadService.GetByIdAsync(id);
            if (actividad == null)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            return Ok(actividad);
        }

        // ✅ GET: api/actividad/usuario/{usuarioRef}
        [HttpGet("usuario/{usuarioRef}")]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetByUsuario(string usuarioRef)
        {
            var actividades = await _actividadService.GetByUsuarioAsync(usuarioRef);
            return Ok(actividades);
        }

        // ✅ GET: api/actividad/pendientes/{usuarioRef}
        [HttpGet("pendientes/{usuarioRef}")]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetPendientesByUsuario(string usuarioRef)
        {
            var pendientes = await _actividadService.GetPendientesPorUsuarioAsync(usuarioRef);
            return Ok(pendientes);
        }

        // ✅ POST: api/actividad
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ActividadCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "Datos inválidos" });

            await _actividadService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { mensaje = "Actividad creada correctamente" });
        }

        // ✅ PUT: api/actividad/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] ActividadCreateDTO dto)
        {
            var actividad = await _actividadService.GetByIdAsync(id);
            if (actividad == null)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            await _actividadService.UpdateAsync(id, dto);
            return Ok(new { mensaje = "Actividad actualizada correctamente" });
        }

        // ✅ DELETE: api/actividad/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var actividad = await _actividadService.GetByIdAsync(id);
            if (actividad == null)
                return NotFound(new { mensaje = "Actividad no encontrada" });

            await _actividadService.DeleteAsync(id);
            return Ok(new { mensaje = "Actividad eliminada correctamente" });
        }
    }
}
