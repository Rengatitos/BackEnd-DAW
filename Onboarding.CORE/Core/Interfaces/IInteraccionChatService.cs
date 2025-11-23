using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IInteraccionChatService
    {
        Task CorregirRespuestaAsync(string id, string nuevaRespuesta);
        Task DeleteAsync(string id);
        Task<InteraccionChatDTO> GenerarInteraccionAsync(InteraccionChatCreateDTO dto);
        Task<List<InteraccionChatDTO>> GetAllAsync();
        Task<InteraccionChatDTO?> GetByIdAsync(string id);
        Task<List<InteraccionChatDTO>> GetByUsuarioAsync(string usuarioRef);
        Task CreateAsync(InteraccionChatCreateDTO dto);
        Task<InteraccionChatDTO?> GetLastByUsuarioAsync(string usuarioRef);
    }
}