using Onboarding.CORE.Entities;
using System.Threading.Tasks;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ISalasChatRepository
    {
        Task<SalaChat?> GetByUsuarioRefAsync(string usuarioRef);
        Task CreateAsync(SalaChat sala);
        Task UpdateAsync(SalaChat sala);
        Task DeleteAsync(string usuarioRef);
    }
}