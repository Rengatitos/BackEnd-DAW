using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs; // Asegúrate de que esta directiva using esté presente

namespace Onboarding.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar Actividades
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ActividadController : ControllerBase
    {
        private readonly IActividadService _actividadService;
        private readonly ILogger<ActividadController> _logger;

        public ActividadController(
            IActividadService actividadService,
            ILogger<ActividadController> logger)
        {
            _actividadService = actividadService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las actividades
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetAll()
        {
            try
            {
                var actividades = await _actividadService.GetAllAsync();
                return Ok(actividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las actividades");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una actividad por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActividadResponseDTO>> GetById(string id)
        {
            try
            {
                var actividad = await _actividadService.GetByIdAsync(id);
                if (actividad == null)
                    return NotFound(new { mensaje = $"No se encontró la actividad con ID {id}" });

                return Ok(actividad);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la actividad {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene actividades por usuario
        /// </summary>
        [HttpGet("usuario/{usuarioRef}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetByUsuario(string usuarioRef)
        {
            try
            {
                var actividades = await _actividadService.GetByUsuarioAsync(usuarioRef);
                return Ok(actividades);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades del usuario {UsuarioRef}", usuarioRef);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene actividades pendientes de un usuario
        /// </summary>
        [HttpGet("pendientes/{usuarioRef}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetPendientesByUsuario(string usuarioRef)
        {
            try
            {
                var pendientes = await _actividadService.GetPendientesPorUsuarioAsync(usuarioRef);
                return Ok(pendientes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pendientes del usuario {UsuarioRef}", usuarioRef);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene actividades por estado
        /// </summary>
        [HttpGet("estado/{estado}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetByEstado(string estado)
        {
            try
            {
                var actividades = await _actividadService.GetByEstadoAsync(estado);
                return Ok(actividades);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades por estado {Estado}", estado);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene actividades en un rango de fechas
        /// </summary>
        [HttpGet("rango-fechas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ActividadResponseDTO>>> GetByFechaRango(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin)
        {
            try
            {
                var actividades = await _actividadService.GetByFechaRangoAsync(fechaInicio, fechaFin);
                return Ok(actividades);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividades por rango de fechas");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el conteo de actividades de un usuario específico
        /// </summary>
        [HttpGet("count/{usuarioRef}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetCountByUsuario(string usuarioRef)
        {
            try
            {
                var count = await _actividadService.GetCountByUsuarioAsync(usuarioRef);
                return Ok(new { usuarioRef, totalActividades = count });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el conteo de actividades del usuario {UsuarioRef}", usuarioRef);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

       
        /// Crea una nueva actividad
       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActividadResponseDTO>> Create([FromBody] ActividadCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actividadCreada = await _actividadService.CreateAsync(dto);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = actividadCreada.Id },
                    actividadCreada
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear actividad");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una actividad existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(string id, [FromBody] ActividadCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actualizado = await _actividadService.UpdateAsync(id, dto);
                if (!actualizado)
                    return NotFound(new { mensaje = $"No se pudo actualizar la actividad con ID {id}" });

                return Ok(new { mensaje = "Actividad actualizada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar actividad {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza solo el estado de una actividad
        /// </summary>
        [HttpPatch("{id}/estado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateEstado(string id, [FromBody] string nuevoEstado)
        {
            try
            {
                var actualizado = await _actividadService.UpdateEstadoAsync(id, nuevoEstado);
                if (!actualizado)
                    return NotFound(new { mensaje = $"No se pudo actualizar el estado de la actividad con ID {id}" });

                return Ok(new { mensaje = "Estado actualizado correctamente", nuevoEstado });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado de actividad {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una actividad
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var eliminado = await _actividadService.DeleteAsync(id);
                if (!eliminado)
                    return NotFound(new { mensaje = $"No se pudo eliminar la actividad con ID {id}" });

                return Ok(new { mensaje = "Actividad eliminada correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar actividad {Id}", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }
    }
}