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

            var updated = await _recursoService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { message = "Recurso no encontrado o id inválido." });

            return Ok(new { message = "Recurso actualizado correctamente." });
        }

        // 🔹 DELETE: api/Recurso/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _recursoService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Recurso no encontrado o id inválido." });

            return Ok(new { message = "Recurso eliminado correctamente." });
        }

        // ==================================================
        // GET: api/Recurso/admin/{adminId}
        // Devuelve todos los recursos asociados a un admin_ref.
        // Si no existen recursos, devuelve lista vacía.
        // ==================================================
        [HttpGet("admin/{adminId}")]
        public async Task<IActionResult> GetByAdmin(string adminId)
        {
            if (string.IsNullOrWhiteSpace(adminId))
                return BadRequest(new { message = "adminId es requerido." });

            var recursos = await _recursoService.GetByAdminAsync(adminId);
            // Retorna lista (vacía si no hay resultados)
            return Ok(recursos);
        }

        // ==================================================
        // PATCH: api/Recurso/{id}/estado
        // Actualiza únicamente el campo 'estado' del recurso.
        // Cuerpo: { "estado": "Inactivo" }
        // Valida que el recurso exista antes de actualizar.
        // ==================================================
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> UpdateEstado(string id, [FromBody] RecursoEstadoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto == null || string.IsNullOrWhiteSpace(dto.Estado))
                return BadRequest(new { message = "El campo 'estado' es requerido." });

            var updated = await _recursoService.UpdateEstadoAsync(id, dto.Estado);
            if (!updated)
                return NotFound(new { message = "Recurso no encontrado o id inválido." });

            return Ok(new { message = "Estado actualizado correctamente." });
        }

        // ==================================================
        // GET: api/Recurso/fecha?desde=yyyy-MM-dd&hasta=yyyy-MM-dd
        // Filtra recursos por rango de fechas (inclusive).
        // Si no se envían fechas, devuelve BadRequest.
        // ==================================================
        [HttpGet("fecha")]
        public async Task<IActionResult> GetByFecha([FromQuery] string? desde, [FromQuery] string? hasta)
        {
            if (string.IsNullOrWhiteSpace(desde) || string.IsNullOrWhiteSpace(hasta))
                return BadRequest(new { message = "Se requieren los query params 'desde' y 'hasta'." });

            if (!DateTime.TryParse(desde, out var desdeDt) || !DateTime.TryParse(hasta, out var hastaDt))
                return BadRequest(new { message = "Formato de fecha inválido. Use yyyy-MM-dd o ISO." });

            if (desdeDt > hastaDt)
                return BadRequest(new { message = "'desde' debe ser anterior o igual a 'hasta'." });

            // Convertir a UTC para comparar con fecha almacenada en UTC
            var desdeUtc = DateTime.SpecifyKind(desdeDt, DateTimeKind.Utc);
            var hastaUtc = DateTime.SpecifyKind(hastaDt, DateTimeKind.Utc);

            var recursos = await _recursoService.GetByFechaRangeAsync(desdeUtc, hastaUtc);
            return Ok(recursos);
        }
    }
}
