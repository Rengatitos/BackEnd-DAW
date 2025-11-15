namespace Onboarding.CORE.DTOs
{
    public class RecursoCreateDTO
    {
        public string Descripcion { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string? AdminRef { get; set; }
    }

    public class RecursoDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; }
    }

    // DTO para actualizar solo el campo 'estado' mediante PATCH
    public class RecursoEstadoDTO
    {
        public string Estado { get; set; } = string.Empty;
    }
}
