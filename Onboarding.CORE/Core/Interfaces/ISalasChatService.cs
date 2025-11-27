using Onboarding.CORE.Core.DTOs;
using Onboarding.CORE.DTOs;
using System.Threading.Tasks;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ISalasChatService
    {
        Task<SalaChatDTO?> GetByUsuarioRefAsync(string usuarioRef);
        Task<SalaChatDTO> CreateAsync(SalaChatCreateDTO dto);
        Task<bool> UpdateEstadoAsync(string usuarioRef, SalaChatUpdateEstadoDTO dto);
        Task<bool> UpdateContextoAsync(string usuarioRef, SalaChatUpdateContextoDTO dto);
        Task<bool> DeleteAsync(string usuarioRef);
        Task<bool> UpdateEstadoAsync(string usuarioRef, CORE.DTOs.SalaChatUpdateEstadoDTO dto);
        Task<List<SalaChatDTO>> GetAllAsync();
    }
}