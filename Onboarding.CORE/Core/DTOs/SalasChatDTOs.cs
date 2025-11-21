using Onboarding.CORE.DTOs;
using System;
using System.Collections.Generic;

namespace Onboarding.CORE.Core.DTOs
{
    public class SalaChatDTO
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioRef { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string RolRef { get; set; } = string.Empty;
        public NivelOnboardingDTO NivelOnboarding { get; set; } = new();
        public EstadoOnboardingIADTO EstadoOnboardingIA { get; set; } = new();
        public string ContextoPersistente { get; set; } = string.Empty;
        public string UltimoMensaje { get; set; } = string.Empty;
        public DateTime UltimaActualizacion { get; set; }
    }
}
