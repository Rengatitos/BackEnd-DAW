using Microsoft.Extensions.Configuration;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IConfiguration _config;

    public UsuarioService(IUsuarioRepository usuarioRepository, IRolRepository rolRepository, IConfiguration config)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
        _config = config;
    }

    public async Task CreateAsync(UsuarioCreateDTO dto)
    {
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RolRef = dto.RolRef,
            Telefono = dto.Telefono,
            Estado = "Activo"
        };

        await _usuarioRepository.CreateAsync(usuario);
    }

    public async Task<UsuarioResponseDTO?> LoginAsync(LoginDTO dto)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);

        if (usuario == null)
            return null;

        bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);

        if (!passwordCorrecta)
            return null;

        var rol = await _rolRepository.GetByIdAsync(usuario.RolRef);

        return new UsuarioResponseDTO
        {
            Id = usuario.Id.ToString(),
            Nombre = usuario.Nombre,
            Correo = usuario.Correo,
            Rol = rol?.Nombre ?? "Sin Rol",
            Telefono = usuario.Telefono
        };
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    public async Task<Usuario?> GetByIdAsync(string id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(string id, UsuarioCreateDTO dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null) return;

        usuario.Nombre = dto.Nombre;
        usuario.Correo = dto.Correo;
        usuario.RolRef = dto.RolRef;
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        usuario.Telefono = dto.Telefono;

        await _usuarioRepository.UpdateAsync(id, usuario);
    }

    public async Task DeleteAsync(string id)
    {
        await _usuarioRepository.DeleteAsync(id);
    }

    public async Task<List<Usuario>> GetByRolRefAsync(string rolRef)
    {
        return await _usuarioRepository.GetByRolRefAsync(rolRef);
    }
}
