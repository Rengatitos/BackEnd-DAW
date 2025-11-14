using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IActividadRepository
    {
        Task CreateAsync(Actividad actividad);
        Task DeleteAsync(string id);
        Task<List<Actividad>> GetAllAsync();
        Task<Actividad?> GetByIdAsync(string id);
        Task<List<Actividad>> GetByUsuarioAsync(string usuarioRef);
        Task UpdateAsync(string id, Actividad actividad);
    }
}