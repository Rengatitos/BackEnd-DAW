using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRecursoRepository
    {
        Task CreateAsync(Recurso recurso);
        Task DeleteAsync(string id);
        Task<List<Recurso>> GetAllAsync();
        Task<Recurso?> GetByIdAsync(string id);
        Task UpdateAsync(string id, Recurso recurso);
    }
}