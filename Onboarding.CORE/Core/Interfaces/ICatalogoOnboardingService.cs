using Onboarding.CORE.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ICatalogoOnboardingService
    {
        Task<CatalogoOnboardingDTO?> GetCatalogoAsync();
        Task<EtapaOnboardingDTO?> GetEtapaAsync(string etapaNombre);
        Task CreateCatalogoAsync(CatalogoOnboardingCreateDTO dto);
        Task<bool> UpdateEtapaAsync(string etapaNombre, CatalogoOnboardingUpdateEtapaDTO dto);
        Task<bool> DeleteEtapaAsync(string etapaNombre);
    }
}