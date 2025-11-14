using System.ComponentModel.DataAnnotations;

namespace Onboarding.CORE.DTOs
{
    /// <summary>
    /// DTO para crear o actualizar una actividad
    /// </summary>
    public class ActividadCreateDTO
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Descripcion { get; set; }

        [StringLength(50, ErrorMessage = "El tipo no puede exceder 50 caracteres")]
        public string? Tipo { get; set; } // NUEVO: curso, tarea, evaluación, etc.

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha fin es requerida")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "La referencia de usuario es requerida")]
        public string? UsuarioRef { get; set; }

        [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
        public string? Estado { get; set; }
    }

    /// <summary>
    /// DTO de respuesta para una actividad
    /// </summary>
    public class ActividadResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // NUEVO
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? UsuarioRef { get; set; } // NUEVO
    }

}