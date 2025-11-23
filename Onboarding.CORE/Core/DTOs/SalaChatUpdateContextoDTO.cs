using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.CORE.DTOs
{
    public class SalaChatUpdateContextoDTO
    {
        public string ContextoPersistente { get; set; } = string.Empty;
        public string UltimoMensaje { get; set; } = string.Empty;
    }
}
