using System;
using System.Collections.Generic;

namespace Onboarding.CORE.DTOs
{
    public class NivelOnboardingDTO
    {
        public string Etapa { get; set; } = string.Empty;
        public int Porcentaje { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class EstadoOnboardingIADTO
    {
        public string PasoActual { get; set; } = string.Empty;
        public bool HaVistoDocumentos { get; set; }
        public bool HaConsultadoSupervisor { get; set; }
        public bool HaSolicitadoAdmin { get; set; }
    }

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

    public class SalaChatCreateDTO
    {
        public string UsuarioRef { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string RolRef { get; set; } = string.Empty;
    }

    public class SalaChatUpdateEstadoDTO
    {
        public NivelOnboardingDTO NivelOnboarding { get; set; } = new();
        public EstadoOnboardingIADTO EstadoOnboardingIA { get; set; } = new();
        public string UltimoMensaje { get; set; } = string.Empty;
    }

    public class SalaChatUpdateContextoDTO
    {
        public string ContextoPersistente { get; set; } = string.Empty;
        public string UltimoMensaje { get; set; } = string.Empty;
    }
}