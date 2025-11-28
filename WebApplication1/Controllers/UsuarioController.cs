using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using System.Threading.Tasks;

namespace Onboarding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;

        // 📌 1. Definimos tus IDs fijos (Copiados de tu imagen de MongoDB)
        private const string ID_ROL_ADMINISTRADOR = "6913adbcca79acfd93858d5c";
        private const string ID_ROL_EMPLEADO = "692284a99875b23f82fb7023";

        public UsuarioController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        // ============================================================
        // 🔐 LOGIN - Lógica de Mapeo de Roles
        // ============================================================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _usuarioService.LoginAsync(dto);
            if (usuario == null)
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });

            // 📌 2. LA JUGADA MAESTRA: Traducir ID -> Palabra
            string nombreRolParaToken = "Usuario"; // Valor por defecto (Empleado)

            if (usuario.Rol == ID_ROL_ADMINISTRADOR)
            {
                nombreRolParaToken = "Administrador";
            }
            else if (usuario.Rol == ID_ROL_EMPLEADO)
            {
                nombreRolParaToken = "Usuario";
            }

            // 📌 3. Generamos el Token con la PALABRA ("Administrador" o "Usuario")
            var token = _jwtService.GenerateToken(
                usuario.Id,
                usuario.Nombre,
                nombreRolParaToken
            );

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                usuario,
                token
            });
        }

        // --- MÉTODO AGREGADO ---
        // ============================================================
        // 🆔 OBTENER USUARIO POR ID (Cualquier usuario autenticado puede acceder)
        // ============================================================
        [HttpGet("{id}")]
        //[Authorize] // <--- Esto requiere que el usuario esté logueado, sin importar el rol.
        public async Task<IActionResult> GetById(string id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(usuario);
        }
        // -------------------------

        // ============================================================
        // 👥 LISTAR USUARIOS (solo ADMIN)
        // ============================================================
        [HttpGet]
        //[Authorize(Roles = "Administrador")] // Descomentar para aplicar restricción
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        // ============================================================
        // 🔎 Obtener usuarios por rolRef 
        // ============================================================
        [HttpGet("rol/{rolRef}")]
        public async Task<IActionResult> GetByRol(string rolRef)
        {
            if (rolRef == "empleado")
                rolRef = ID_ROL_EMPLEADO;
            else if (rolRef == "administrador")
                rolRef = ID_ROL_ADMINISTRADOR;

            var usuarios = await _usuarioService.GetByRolRefAsync(rolRef);
            return Ok(usuarios);
        }

        // ============================================================
        // ➕ CREAR NUEVO USUARIO (solo ADMIN)
        // ============================================================
        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
        {
            await _usuarioService.CreateAsync(dto);
            return Ok(new { message = "Usuario creado correctamente" });
        }


        // ============================================================
        // ✏️ ACTUALIZAR USUARIO
        // ============================================================
        [HttpPut("{id}")]
      
        public async Task<IActionResult> Update(string id, [FromBody] UsuarioCreateDTO dto)
        {
            var existingUser = await _usuarioService.GetByIdAsync(id);
            if (existingUser == null) return NotFound(new { message = "Usuario no encontrado" });

            await _usuarioService.UpdateAsync(id, dto);
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        // ============================================================
        // 🗑️ ELIMINAR USUARIO
        // ============================================================
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(string id)
        {
            var existingUser = await _usuarioService.GetByIdAsync(id);
            if (existingUser == null) return NotFound(new { message = "Usuario no encontrado" });

            await _usuarioService.DeleteAsync(id);
            return Ok(new { message = "Usuario eliminado correctamente" });
        }
    }
}