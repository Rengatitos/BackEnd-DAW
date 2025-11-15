using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;

public interface IUsuarioService
{
    Task CreateAsync(UsuarioCreateDTO dto);
    Task<UsuarioResponseDTO?> LoginAsync(LoginDTO dto);
    Task<List<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(string id);
    Task UpdateAsync(string id, UsuarioCreateDTO dto);
    Task DeleteAsync(string id);
}
