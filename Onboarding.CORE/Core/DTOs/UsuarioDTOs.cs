namespace Onboarding.CORE.DTOs
{
    public class UsuarioCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // <-- Corregido
        public string? RolRef { get; set; }
        public string? Telefono { get; set; } // <-- agregado
    }


    public class LoginDTO
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string? Telefono { get; set; } // <-- agregado
    }
}
