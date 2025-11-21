using System.Threading.Tasks;
using System.Collections.Generic;
using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ICatalogoOnboardingService
    {
        Task<CatalogoOnboardingDTO?> GetCatalogoAsync();
        Task<EtapaOnboardingDTO?> GetEtapaAsync(string etapaNombre);
        Task CreateCatalogoAsync(CatalogoOnboardingCreateItemDTO dto);
        Task<bool> UpdateEtapaAsync(string etapaNombre, CatalogoOnboardingUpdateEtapaDTO dto);
        Task<bool> DeleteEtapaAsync(string etapaNombre);
    }
}