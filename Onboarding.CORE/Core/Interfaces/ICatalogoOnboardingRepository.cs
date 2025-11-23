using System.Threading.Tasks;
using System.Collections.Generic;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ICatalogoOnboardingRepository
    {
        Task<Onboarding.CORE.Core.Entities.CatalogoOnboarding?> GetCatalogoAsync();
        Task<Onboarding.CORE.Core.Entities.EtapaOnboarding?> GetEtapaAsync(string etapaNombre);
        Task CreateCatalogoAsync(Onboarding.CORE.Core.Entities.CatalogoOnboarding catalogo);
        Task UpdateEtapaAsync(string etapaNombre, Onboarding.CORE.Core.Entities.EtapaOnboarding etapa);
        Task DeleteEtapaAsync(string etapaNombre);
    }
}