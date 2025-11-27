using Onboarding.CORE.Core.Entities;
using Onboarding.CORE.Entities;
using System.Threading.Tasks;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface ISalasChatRepository
    {
        Task<SalaChat?> GetByUsuarioRefAsync(string usuarioRef);
        Task CreateAsync(SalaChat salaChat);
        Task<bool> UpdateEstadoAsync(string usuarioRef, string nuevoEstado);
        Task<bool> UpdateContextoAsync(string usuarioRef, string nuevoContexto);
        Task UpdateAsync(SalaChat entity);
        Task DeleteAsync(string usuarioRef);
        Task<List<SalaChat>> GetAllAsync(); // <--- Agregar esta línea
    }
}