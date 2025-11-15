using Onboarding.CORE.Entities;

namespace Onboarding.CORE.Core.Interfaces
{
    /// <summary>
    /// Interface para el repositorio de Actividades
    /// </summary>
    public interface IActividadRepository
    {
        // MÉTODOS ORIGINALES CON FIRMAS ACTUALIZADAS
        Task<List<Actividad>> GetAllAsync();
        Task<Actividad?> GetByIdAsync(string id);
        Task<List<Actividad>> GetByUsuarioAsync(string usuarioRef);
        Task<Actividad> CreateAsync(Actividad actividad); // CAMBIO: ahora retorna Actividad
        Task<bool> UpdateAsync(string id, Actividad actividad); // CAMBIO: ahora retorna bool
        Task<bool> DeleteAsync(string id); // CAMBIO: ahora retorna bool

        // NUEVOS MÉTODOS
        Task<List<Actividad>> GetByEstadoAsync(string estado);
        Task<List<Actividad>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<bool> UpdateEstadoAsync(string id, string nuevoEstado);
        Task<bool> ExistsAsync(string id);
        Task<long> GetCountByUsuarioAsync(string usuarioRef);

    }
}