using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRolService
    {
        Task<RolDTO> CreateAsync(RolCreateDTO dto);
        Task DeleteAsync(string id);
        Task<IEnumerable<RolDTO>> GetAllAsync();
        Task<RolDTO?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, RolCreateDTO dto);
    }
}