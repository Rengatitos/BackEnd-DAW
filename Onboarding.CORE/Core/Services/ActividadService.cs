using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Services
{
    /// <summary>
    /// Servicio para la lógica de negocio de Actividades
    /// </summary>
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _actividadRepository;

        public ActividadService(IActividadRepository actividadRepository)
        {
            _actividadRepository = actividadRepository;
        }

        /// <summary>
        /// Obtiene todas las actividades
        /// </summary>
        public async Task<List<ActividadResponseDTO>> GetAllAsync()
        {
            var actividades = await _actividadRepository.GetAllAsync();
            return actividades.Select(MapToResponse).ToList();
        }

        /// <summary>
        /// Obtiene actividades por usuario
        /// </summary>
        public async Task<List<ActividadResponseDTO>> GetByUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef))
                throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioRef));

            var actividades = await _actividadRepository.GetByUsuarioAsync(usuarioRef);
            return actividades.Select(MapToResponse).ToList();
        }

        /// <summary>
        /// Obtiene una actividad por ID
        /// </summary>
        public async Task<ActividadResponseDTO?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            var actividad = await _actividadRepository.GetByIdAsync(id);
            return actividad == null ? null : MapToResponse(actividad);
        }

        /// <summary>
        /// Obtiene actividades por estado
        /// </summary>
        public async Task<List<ActividadResponseDTO>> GetByEstadoAsync(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado no puede estar vacío", nameof(estado));

            var actividades = await _actividadRepository.GetByEstadoAsync(estado);
            return actividades.Select(MapToResponse).ToList();
        }

        /// <summary>
        /// Obtiene actividades en un rango de fechas
        /// </summary>
        public async Task<List<ActividadResponseDTO>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio debe ser menor o igual a la fecha fin");

            var actividades = await _actividadRepository.GetByFechaRangoAsync(fechaInicio, fechaFin);
            return actividades.Select(MapToResponse).ToList();
        }

        /// <summary>
        /// Crea una nueva actividad con validaciones
        /// </summary>
        public async Task<ActividadResponseDTO> CreateAsync(ActividadCreateDTO dto)
        {
            ValidarDTO(dto);

            // ✅ CAMBIO AQUÍ
            if (dto.FechaInicio > dto.FechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha fin");

            var actividad = new Actividad
            {
                Titulo = dto.Titulo.Trim(),
                Descripcion = dto.Descripcion?.Trim() ?? string.Empty,
                Tipo = dto.Tipo ?? "curso",
                FechaInicio = dto.FechaInicio.ToUniversalTime(),
                FechaFin = dto.FechaFin.ToUniversalTime(),
                UsuarioRef = dto.UsuarioRef!,
                Estado = dto.Estado ?? "En Proceso"
            };

            try
            {
                var actividadCreada = await _actividadRepository.CreateAsync(actividad);
                return MapToResponse(actividadCreada);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al crear la actividad: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza una actividad existente
        /// </summary>
        public async Task<bool> UpdateAsync(string id, ActividadCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            ValidarDTO(dto);

            var existe = await _actividadRepository.ExistsAsync(id);
            if (!existe)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {id}");

            // ✅ CAMBIO AQUÍ
            if (dto.FechaInicio > dto.FechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha fin");

            var actividad = new Actividad
            {
                Id = id,
                Titulo = dto.Titulo.Trim(),
                Descripcion = dto.Descripcion?.Trim() ?? string.Empty,
                Tipo = dto.Tipo ?? "curso",
                FechaInicio = dto.FechaInicio.ToUniversalTime(),
                FechaFin = dto.FechaFin.ToUniversalTime(),
                UsuarioRef = dto.UsuarioRef!,
                Estado = dto.Estado ?? "En Proceso"
            };

            try
            {
                return await _actividadRepository.UpdateAsync(id, actividad);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al actualizar la actividad: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Actualiza solo el estado de una actividad
        /// </summary>
        public async Task<bool> UpdateEstadoAsync(string id, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new ArgumentException("El estado no puede estar vacío", nameof(nuevoEstado));

            var estadosPermitidos = new[] { "Pendiente", "En Proceso", "Completado", "Cancelado" };
            if (!estadosPermitidos.Contains(nuevoEstado, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException($"Estado inválido. Estados permitidos: {string.Join(", ", estadosPermitidos)}");

            var existe = await _actividadRepository.ExistsAsync(id);
            if (!existe)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {id}");

            return await _actividadRepository.UpdateEstadoAsync(id, nuevoEstado);
        }

        /// <summary>
        /// Elimina una actividad
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            // CAMBIO: Ahora retorna bool
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            var existe = await _actividadRepository.ExistsAsync(id);
            if (!existe)
                throw new KeyNotFoundException($"No se encontró la actividad con ID {id}");

            try
            {
                return await _actividadRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar la actividad: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene actividades pendientes de un usuario
        /// </summary>
        public async Task<List<ActividadResponseDTO>> GetPendientesPorUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef))
                throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioRef));

            var actividades = await _actividadRepository.GetByUsuarioAsync(usuarioRef);
            var pendientes = actividades
                .Where(a => a.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                .Select(MapToResponse)
                .ToList();

            return pendientes;
        }

        /// <summary>
        /// Obtiene el conteo de actividades de un usuario específico
        /// </summary>
        public async Task<long> GetCountByUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef))
                throw new ArgumentException("El ID de usuario no puede estar vacío", nameof(usuarioRef));

            return await _actividadRepository.GetCountByUsuarioAsync(usuarioRef);
        }



        /// <summary>
        /// Valida el DTO de entrada
        /// </summary>
        private static void ValidarDTO(ActividadCreateDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "El DTO no puede ser nulo");

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new ArgumentException("El título es requerido", nameof(dto.Titulo));

            if (dto.Titulo.Length < 3 || dto.Titulo.Length > 200)
                throw new ArgumentException("El título debe tener entre 3 y 200 caracteres");

            if (dto.Descripcion != null && dto.Descripcion.Length > 1000)
                throw new ArgumentException("La descripción no puede exceder 1000 caracteres");

            if (string.IsNullOrWhiteSpace(dto.UsuarioRef))
                throw new ArgumentException("La referencia de usuario es requerida", nameof(dto.UsuarioRef));
        }

        /// <summary>
        /// Mapea de entidad a DTO de respuesta
        /// </summary>
        private static ActividadResponseDTO MapToResponse(Actividad a) => new()
        {
            Id = a.Id!,
            Titulo = a.Titulo,
            Descripcion = a.Descripcion,
            Tipo = a.Tipo, // NUEVO
            Estado = a.Estado,
            FechaInicio = a.FechaInicio,
            FechaFin = a.FechaFin,
            UsuarioRef = a.UsuarioRef // NUEVO
        };
    }
}