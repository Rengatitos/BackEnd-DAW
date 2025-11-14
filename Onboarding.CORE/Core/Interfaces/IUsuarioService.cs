using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task CreateAsync(UsuarioCreateDTO dto);
        Task DeleteAsync(string id);
        Task<List<UsuarioResponseDTO>> GetAllAsync();
        Task<UsuarioResponseDTO?> GetByIdAsync(string id);
        Task<UsuarioResponseDTO?> LoginAsync(LoginDTO dto);
        Task UpdateAsync(string id, UsuarioCreateDTO dto);
    }
}