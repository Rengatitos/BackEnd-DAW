using Onboarding.CORE.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.CORE.Core.DTOs
{
    internal class CatalogoOnboardingCreateDTO
    {
        public List<EtapaOnboardingCreateItemDTO> Etapas { get; set; } = new();
    }
}

