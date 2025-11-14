using Onboarding.CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRecursoRepository
    {
        Task CreateAsync(Recurso recurso);
        Task DeleteAsync(string id);
        Task<List<Recurso>> GetAllAsync();
        Task<Recurso?> GetByIdAsync(string id);
        Task UpdateAsync(string id, Recurso recurso);

        // Nuevos métodos
        Task<List<Recurso>> GetByAdminRefAsync(string adminId);
        Task<bool> UpdateEstadoAsync(string id, string estado);
        Task<List<Recurso>> GetByFechaRangeAsync(DateTime desde, DateTime hasta);
    }
}