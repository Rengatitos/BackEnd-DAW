using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IInteraccionChatRepository
    {
        Task<List<InteraccionChat>> GetAllAsync();
        Task<List<InteraccionChat>> GetByUsuarioAsync(string usuarioRef);
        Task<InteraccionChat?> GetByIdAsync(string id);
        Task<InteraccionChat?> BuscarPorPreguntaAsync(string mensajeUsuario);
        Task CreateAsync(InteraccionChat interaccion);
        Task UpdateAsync(InteraccionChat interaccion);
        Task DeleteAsync(string id);
        Task<InteraccionChat?> GetLastByUsuarioAsync(string usuarioRef);
    }
}
