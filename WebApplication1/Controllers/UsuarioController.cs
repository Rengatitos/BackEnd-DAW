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

        public UsuarioController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        // ============================================================
        // 🔐 LOGIN - SIN AUTORIZACIÓN
        // ============================================================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _usuarioService.LoginAsync(dto);
            if (usuario == null)
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });

            var token = _jwtService.GenerateToken(
                usuario.Id,
                usuario.Nombre,
                usuario.Rol
            );

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                usuario,
                token
            });
        }

        // ============================================================
        // 👥 LISTAR USUARIOS (solo ADMIN)
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        // ============================================================
        // 🔎 Obtener usuarios por rolRef (acepta 'empleado' y 'administrador' como alias)
        // ============================================================
        [HttpGet("rol/{rolRef}")]
        public async Task<IActionResult> GetByRol(string rolRef)
        {
            // Mapear alias a IDs específicos
            if (rolRef == "empleado")
                rolRef = "6913adbcca79acfd93858d5d";
            else if (rolRef == "administrador")
                rolRef = "6913adbcca79acfd93858d5c";

            var usuarios = await _usuarioService.GetByRolRefAsync(rolRef);
            return Ok(usuarios);
        }

        // ============================================================
        // ➕ CREAR NUEVO USUARIO (solo ADMIN)
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
        {
            await _usuarioService.CreateAsync(dto);
            return Ok(new { message = "Usuario creado correctamente" });
        }


        // ============================================================
        // ✏️ ACTUALIZAR USUARIO
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> Update(string id, [FromBody] UsuarioCreateDTO dto)
        {
            // 1. Validar que el usuario exista
            var existingUser = await _usuarioService.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // 2. Actualizar
            await _usuarioService.UpdateAsync(id, dto);
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        // ============================================================
        // 🗑️ ELIMINAR USUARIO
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(string id)
        {
            // 1. Validar que el usuario exista
            var existingUser = await _usuarioService.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // 2. Eliminar
            await _usuarioService.DeleteAsync(id);
            return Ok(new { message = "Usuario eliminado correctamente" });
        }




    }
}