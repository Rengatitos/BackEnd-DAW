using Onboarding.CORE.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ICatalogoOnboardingRepository
    {
        Task<CatalogoOnboarding?> GetCatalogoAsync();
        Task<CatalogoOnboarding?> GetEtapaAsync(string etapaNombre);
        Task CreateCatalogoAsync(CatalogoOnboarding catalogo);
        Task UpdateEtapaAsync(string etapaNombre, EtapaOnboarding etapa);
        Task DeleteEtapaAsync(string etapaNombre);
    }
}