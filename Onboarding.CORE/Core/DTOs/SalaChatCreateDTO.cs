using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onboarding.CORE.DTOs
{
    public class SalaChatCreateDTO
    {
        public string UsuarioRef { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string RolRef { get; set; } = string.Empty;
    }
}
