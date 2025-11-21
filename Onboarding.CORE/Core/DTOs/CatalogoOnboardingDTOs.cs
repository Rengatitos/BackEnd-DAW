using System.Collections.Generic;
using System;

namespace Onboarding.CORE.DTOs
{
    public class EtapaOnboardingDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<string> Documentos { get; set; } = new();
        public List<string> Urls { get; set; } = new();
        public List<string> ProximosPasos { get; set; } = new();
        public List<string> Consejos { get; set; } = new();
    }

    public class CatalogoOnboardingDTO
    {
        public string Id { get; set; } = string.Empty;
        public List<EtapaOnboardingDTO> Etapas { get; set; } = new();
    }

    public class CatalogoOnboardingCreateItemDTO
    {
        public List<EtapaOnboardingDTO> Etapas { get; set; } = new();
    }

    public class CatalogoOnboardingUpdateEtapaDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<string> Documentos { get; set; } = new();
        public List<string> Urls { get; set; } = new();
        public List<string> ProximosPasos { get; set; } = new();
        public List<string> Consejos { get; set; } = new();
    }
}