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

        public InteraccionChatController(
            IInteraccionChatService chatService,
            OllamaClient ollamaClient)
        {
            _chatService = chatService;
            _ollamaClient = ollamaClient;
        }

        /// <summary>
        /// Procesa un mensaje del usuario, obtiene respuesta de Ollama
        /// y registra la interacción en MongoDB.
        /// </summary>
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] InteraccionChatCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "El body no puede ser null." });

            if (string.IsNullOrWhiteSpace(dto.MensajeUsuario))
                return BadRequest(new { message = "El mensaje no puede estar vacío." });

            try
            {
                // 🔥 Llamada correcta (PowerShell style)
                string respuesta = await _ollamaClient.GenerarRespuestaAsync(dto.MensajeUsuario);

                var interaccionDto = new InteraccionChatCreateDTO
                {
                    UsuarioRef = dto.UsuarioRef ?? string.Empty,
                    MensajeUsuario = dto.MensajeUsuario,
                    RespuestaChatbot = respuesta,
                    Contexto = dto.Contexto ?? string.Empty
                };

                await _chatService.CreateAsync(interaccionDto);

                return Ok(new
                {
                    mensaje_usuario = dto.MensajeUsuario,
                    respuesta_chatbot = respuesta,
                    contexto = dto.Contexto,
                    guardado = "✅ Interacción registrada en MongoDB"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al procesar el chat.",
                    detalle = ex.Message
                });
            }
        }
    }
    }
