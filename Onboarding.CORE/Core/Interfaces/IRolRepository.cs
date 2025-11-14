using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRolRepository
    {
        Task CreateAsync(Rol rol);
        Task DeleteAsync(string id);
        Task<List<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(string id);
        Task<Rol?> GetByNombreAsync(string nombre);
        Task UpdateAsync(string id, Rol rol);
    }
}