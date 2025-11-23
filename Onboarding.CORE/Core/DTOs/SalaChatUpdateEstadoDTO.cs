using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.CORE.DTOs
{
    public class SalaChatUpdateEstadoDTO
    {
        public NivelOnboardingDTO NivelOnboarding { get; set; } = new();
        public EstadoOnboardingIADTO EstadoOnboardingIA { get; set; } = new();
        public string UltimoMensaje { get; set; } = string.Empty;
    }
}
