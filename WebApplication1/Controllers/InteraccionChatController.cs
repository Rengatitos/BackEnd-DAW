using Microsoft.AspNetCore.Mvc;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Helpers;

namespace Onboarding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteraccionChatController : ControllerBase
    {
        private readonly IInteraccionChatService _chatService;
        private readonly OllamaClient _ollamaClient;

        public InteraccionChatController(IInteraccionChatService chatService)
        {
            _chatService = chatService;
            _ollamaClient = new OllamaClient(); // Cliente para llamar al modelo local
        }

        // POST: api/InteraccionChat/chat
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] InteraccionChatCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MensajeUsuario))
                return BadRequest(new { message = "El mensaje no puede estar vacío." });

            try
            {
                // 🔹 Generar respuesta usando OllamaClient con tinyllama
                string respuesta = await _ollamaClient.GenerarRespuestaAsync(dto.MensajeUsuario, "tinyllama");

                // 🔹 Guardar la interacción en MongoDB
                var interaccionDto = new InteraccionChatCreateDTO
                {
                    UsuarioRef = dto.UsuarioRef ?? string.Empty,
                    MensajeUsuario = dto.MensajeUsuario ?? string.Empty,
                    RespuestaChatbot = respuesta ?? string.Empty,
                    Contexto = dto.Contexto ?? string.Empty
                };

                await _chatService.CreateAsync(interaccionDto);

                // 🔹 Retornar la respuesta generada al cliente
                return Ok(new
                {
                    usuario = dto.MensajeUsuario,
                    respuesta = respuesta,
                    contexto = dto.Contexto,
                    guardado = "✅ Interacción registrada en MongoDB"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al procesar el chat",
                    detalle = ex.Message
                });
            }
        }
    }
}
