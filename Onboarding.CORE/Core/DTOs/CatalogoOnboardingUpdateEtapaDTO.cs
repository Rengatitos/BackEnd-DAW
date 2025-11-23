using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.CORE.Core.DTOs
{
    internal class CatalogoOnboardingUpdateEtapaDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public List<string>? Documentos { get; set; } = new();
        public List<string>? Urls { get; set; } = new();
        public List<string>? ProximosPasos { get; set; } = new();
        public List<string>? Consejos { get; set; } = new();
    }
}
