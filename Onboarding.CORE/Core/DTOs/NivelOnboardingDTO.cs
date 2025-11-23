using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Onboarding.CORE.DTOs
{
    public class NivelOnboardingDTO
    {
        public string Etapa { get; set; } = "Inicial";
        public int Porcentaje { get; set; } = 0;
        public DateTime UltimaActualizacion { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
