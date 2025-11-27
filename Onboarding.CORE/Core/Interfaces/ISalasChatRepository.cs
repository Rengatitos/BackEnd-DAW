using Onboarding.CORE.Core.Entities;
using Onboarding.CORE.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ISalasChatRepository
    {
        Task<List<SalaChat>> GetAllAsync();
        Task<SalaChat?> GetByUsuarioRefAsync(string usuarioRef);
        Task CreateAsync(SalaChat salaChat);
        Task<bool> UpdateEstadoAsync(string usuarioRef, string nuevoEstado);
        Task<bool> UpdateContextoAsync(string usuarioRef, string nuevoContexto);
        Task UpdateAsync(SalaChat entity);
        Task DeleteAsync(string usuarioRef);
    }
}