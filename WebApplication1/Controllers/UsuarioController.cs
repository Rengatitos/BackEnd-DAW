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
        // 🔍 OBTENER USUARIO POR ID (solo ADMIN)
        // ============================================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetById(string id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(usuario);
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
        // 📝 ACTUALIZAR USUARIO (solo ADMIN)
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(string id, [FromBody] UsuarioCreateDTO dto)
        {
            await _usuarioService.UpdateAsync(id, dto);
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        // ============================================================
        // 🗑️ ELIMINAR USUARIO (solo ADMIN)
        // ============================================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(string id)
        {
            await _usuarioService.DeleteAsync(id);
            return Ok(new { message = "Usuario eliminado correctamente" });
        }
    }
}
