using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task CreateAsync(Usuario usuario);
        Task DeleteAsync(string id);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<Usuario?> GetByIdAsync(string id);
        Task UpdateAsync(string id, Usuario usuario);
        Task<List<Usuario>> GetByRolRefAsync(string rolRef);
    }
}