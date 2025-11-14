using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Services
{
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _actividadRepository;

        public ActividadService(IActividadRepository actividadRepository)
        {
            _actividadRepository = actividadRepository;
        }

        public async Task<List<ActividadResponseDTO>> GetAllAsync()
        {
            var actividades = await _actividadRepository.GetAllAsync();
            return actividades.Select(MapToResponse).ToList();
        }

        public async Task<List<ActividadResponseDTO>> GetByUsuarioAsync(string usuarioRef)
        {
            var actividades = await _actividadRepository.GetByUsuarioAsync(usuarioRef);
            return actividades.Select(MapToResponse).ToList();
        }

        public async Task<ActividadResponseDTO?> GetByIdAsync(string id)
        {
            var actividad = await _actividadRepository.GetByIdAsync(id);
            return actividad == null ? null : MapToResponse(actividad);
        }

        public async Task CreateAsync(ActividadCreateDTO dto)
        {
            var actividad = new Actividad
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                UsuarioRef = dto.UsuarioRef!,
                Estado = dto.Estado
            };

            await _actividadRepository.CreateAsync(actividad);
        }

        public async Task UpdateAsync(string id, ActividadCreateDTO dto)
        {
            var actividad = new Actividad
            {
                Id = id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                UsuarioRef = dto.UsuarioRef!,
                Estado = dto.Estado
            };

            await _actividadRepository.UpdateAsync(id, actividad);
        }

        public async Task DeleteAsync(string id)
        {
            await _actividadRepository.DeleteAsync(id);
        }

        // ✅ Nuevo método corregido
        public async Task<List<ActividadResponseDTO>> GetPendientesPorUsuarioAsync(string usuarioRef)

        {
            var actividades = await _actividadRepository.GetByUsuarioAsync(usuarioRef);
            var pendientes = actividades
                .Where(a => a.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                .Select(MapToResponse)
                .ToList();

            return pendientes;
        }

        private static ActividadResponseDTO MapToResponse(Actividad a) => new()
        {
            Id = a.Id!,
            Titulo = a.Titulo,
            Descripcion = a.Descripcion,
            Estado = a.Estado,
            FechaInicio = a.FechaInicio,
            FechaFin = a.FechaFin
        };
    }
}
