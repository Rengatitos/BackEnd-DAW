using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;


namespace Onboarding.CORE.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IRolRepository rolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
        }

        public async Task<List<UsuarioResponseDTO>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            var result = new List<UsuarioResponseDTO>();

            foreach (var u in usuarios)
            {
                var rol = await _rolRepository.GetByIdAsync(u.RolRef ?? "");
                result.Add(new UsuarioResponseDTO
                {
                    Id = u.Id!,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    Rol = rol?.Nombre ?? "Sin Rol"
                });
            }
            return result;
        }

        public async Task<UsuarioResponseDTO?> GetByIdAsync(string id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            var rol = await _rolRepository.GetByIdAsync(usuario.RolRef ?? "");
            return new UsuarioResponseDTO
            {
                Id = usuario.Id!,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = rol?.Nombre ?? "Sin Rol"
            };
        }

        public async Task<UsuarioResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);
            if (usuario == null || usuario.PasswordHash != dto.Password)
                return null;

            var rol = await _rolRepository.GetByIdAsync(usuario.RolRef ?? "");
            return new UsuarioResponseDTO
            {
                Id = usuario.Id!,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = rol?.Nombre ?? "Sin Rol"
            };
        }

        public async Task CreateAsync(UsuarioCreateDTO dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                PasswordHash = dto.PasswordHash,
                RolRef = dto.RolRef,
                Estado = "Activo"
            };
            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task UpdateAsync(string id, UsuarioCreateDTO dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                PasswordHash = dto.PasswordHash,
                RolRef = dto.RolRef
            };
            await _usuarioRepository.UpdateAsync(id, usuario);
        }

        public async Task DeleteAsync(string id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }
    }
}
