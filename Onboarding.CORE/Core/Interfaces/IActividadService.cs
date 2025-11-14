using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IActividadService
    {
        Task CreateAsync(ActividadCreateDTO dto);
        Task DeleteAsync(string id);
        Task<List<ActividadResponseDTO>> GetAllAsync();
        Task<ActividadResponseDTO?> GetByIdAsync(string id);
        Task<List<ActividadResponseDTO>> GetByUsuarioAsync(string usuarioRef);

        // ✅ Asegúrate de usar el mismo nombre que en el service
        Task<List<ActividadResponseDTO>> GetPendientesPorUsuarioAsync(string usuarioRef);

        Task UpdateAsync(string id, ActividadCreateDTO dto);
    }
}
