namespace Onboarding.CORE.DTOs
{
    public class RolCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<string> Permisos { get; set; } = new();
    }

    public class RolDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<string> Permisos { get; set; } = new();
    }
}
