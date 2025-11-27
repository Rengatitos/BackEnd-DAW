using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Core.Interfaces
{
    /// <summary>
    /// Interface para el servicio de Actividades
    /// </summary>
    public interface IActividadService
    {
        // MÉTODOS ORIGINALES CON FIRMAS ACTUALIZADAS
        Task<List<ActividadResponseDTO>> GetAllAsync();
        Task<ActividadResponseDTO?> GetByIdAsync(string id);
        Task<List<ActividadResponseDTO>> GetByUsuarioAsync(string usuarioRef);
        Task<List<ActividadResponseDTO>> GetPendientesPorUsuarioAsync(string usuarioRef);
        Task<ActividadResponseDTO> CreateAsync(ActividadCreateDTO dto); // CAMBIO: retorna ActividadResponseDTO
        Task<bool> UpdateAsync(string id, ActividadCreateDTO dto); // CAMBIO: retorna bool
        Task<bool> DeleteAsync(string id); // CAMBIO: retorna bool

        // NUEVOS MÉTODOS
        Task<List<ActividadResponseDTO>> GetByEstadoAsync(string estado);
        Task<List<ActividadResponseDTO>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<bool> UpdateEstadoAsync(string id, string nuevoEstado);
        Task<long> GetCountByUsuarioAsync(string usuarioRef);
        Task<object> GetProgresoGlobalAsync();

    }
}