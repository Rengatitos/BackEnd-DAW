using Onboarding.CORE.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Onboarding.CORE.Core.Interfaces
{
    public interface IRecursoService
    {
        Task<RecursoDTO> CreateAsync(RecursoCreateDTO dto);
        Task<bool> DeleteAsync(string id);
        Task<List<RecursoDTO>> GetAllAsync();
        Task<RecursoDTO?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, RecursoCreateDTO dto);

        // Nuevos métodos
        Task<List<RecursoDTO>> GetByAdminAsync(string adminId);
        Task<bool> UpdateEstadoAsync(string id, string estado);
        Task<List<RecursoDTO>> GetByFechaRangeAsync(DateTime desde, DateTime hasta);
    }
}