using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRecursoService
    {
        Task<RecursoDTO> CreateAsync(RecursoCreateDTO dto);
        Task<bool> DeleteAsync(string id);
        Task<List<RecursoDTO>> GetAllAsync();
        Task<RecursoDTO?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, RecursoCreateDTO dto);
    }
}