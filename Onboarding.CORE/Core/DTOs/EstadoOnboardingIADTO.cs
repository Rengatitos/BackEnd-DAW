using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Onboarding.CORE.DTOs
{
    public class EstadoOnboardingIADTO
    {
        public string PasoActual { get; set; } = "Inicial";
        public bool HaVistoDocumentos { get; set; }
        public bool HaConsultadoSupervisor { get; set; }
        public bool HaSolicitadoAdmin { get; set; }
    }
}
