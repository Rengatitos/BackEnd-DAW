namespace Onboarding.CORE.DTOs
{
    public class InteraccionChatDTO
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioRef { get; set; } = string.Empty;
        public string MensajeUsuario { get; set; } = string.Empty;
        public string RespuestaChatbot { get; set; } = string.Empty;
        public string? RespuestaCorregida { get; set; }
        public bool EsCorregida { get; set; }
        public DateTime FechaHora { get; set; }
        public string Contexto { get; set; } = string.Empty;
    }

    public class InteraccionChatCreateDTO
    {
        public string UsuarioRef { get; set; } = string.Empty;
        public string MensajeUsuario { get; set; } = string.Empty;
        public string RespuestaChatbot { get; set; } = string.Empty;
        public string Contexto { get; set; } = string.Empty;
    }
}
