using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;


namespace Onboarding.CORE.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<IEnumerable<RolDTO>> GetAllAsync()
        {
            var roles = await _rolRepository.GetAllAsync();
            return roles.Select(r => new RolDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Permisos = r.Permisos
            });
        }

        public async Task<RolDTO?> GetByIdAsync(string id)
        {
            var rol = await _rolRepository.GetByIdAsync(id);
            if (rol == null) return null;

            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Permisos = rol.Permisos
            };
        }

        public async Task<RolDTO> CreateAsync(RolCreateDTO dto)
        {
            var rol = new Rol
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Permisos = dto.Permisos
            };

            await _rolRepository.CreateAsync(rol);

            return new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Permisos = rol.Permisos
            };
        }

        public async Task<bool> UpdateAsync(string id, RolCreateDTO dto)
        {
            var existing = await _rolRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Nombre = dto.Nombre;
            existing.Descripcion = dto.Descripcion;
            existing.Permisos = dto.Permisos;

            await _rolRepository.UpdateAsync(id, existing);
            return true;
        }

        public async Task DeleteAsync(string id)
        {
            await _rolRepository.DeleteAsync(id);
        }
    }
}
