using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Onboarding.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteraccionChatController : ControllerBase
    {
        private readonly IInteraccionChatService _chatService;
        private readonly OllamaClient _ollamaClient;
        private readonly ILogger<InteraccionChatController> _logger;

        public InteraccionChatController(
            IInteraccionChatService chatService,
            OllamaClient ollamaClient,
            ILogger<InteraccionChatController> logger)
        {
            _chatService = chatService;
            _ollamaClient = ollamaClient;
            _logger = logger;
        }

        /// <summary>
        /// Procesa un mensaje del usuario, obtiene respuesta de Ollama (prompt ligero),
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
                dto.UsuarioRef ??= string.Empty;
                dto.Contexto ??= string.Empty;

                // Obtener SOLO el último mensaje para no saturar al modelo
                var ultimo = await _chatService.GetLastByUsuarioAsync(dto.UsuarioRef);
                string ultimoMensaje = ultimo?.MensajeUsuario ?? "";

                // === PROMPT LIGERO ===
                string prompt = ConstruirPromptLigero(
                    dto.Contexto,
                    ultimoMensaje,
                    dto.MensajeUsuario
                );

                _logger.LogInformation("Enviando prompt ligero a Ollama para usuario {user}", dto.UsuarioRef);

                // ===== OLLAMA =====
                string respuesta = await _ollamaClient.GenerarRespuestaAsync(prompt);

                // Guardar interacción
                var interaccionDto = new InteraccionChatCreateDTO
                {
                    UsuarioRef = dto.UsuarioRef,
                    MensajeUsuario = dto.MensajeUsuario,
                    RespuestaChatbot = respuesta,
                    Contexto = dto.Contexto
                };

                await _chatService.CreateAsync(interaccionDto);

                return Ok(new
                {
                    mensaje_usuario = dto.MensajeUsuario,
                    respuesta_chatbot = respuesta,
                    guardado = true
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el chat para usuario {user}", dto?.UsuarioRef);

                return StatusCode(500, new
                {
                    message = "Error al procesar el chat.",
                    detalle = ex.Message
                });
            }
        }

        // ============================================================
        // PROMPT LIGERO (OPTIMIZADO PARA MODELOS 3B)
        // ============================================================
        private string ConstruirPromptLigero(string contexto, string ultimoMensaje, string mensajeNuevo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Eres un asistente virtual para onboarding de una empresa. Responde SIEMPRE en español, con máximo 2 líneas.");
            sb.AppendLine();
            sb.AppendLine("Funciones del asistente:");
            sb.AppendLine("- Presentar opciones múltiples cuando sea útil.");
            sb.AppendLine("- Proveer enlaces cuando el usuario lo necesite.");
            sb.AppendLine("- Redirigir a un administrador cuando sea solicitado.");
            sb.AppendLine("- No inventar fechas exactas ni detalles que no conozcas.");
            sb.AppendLine("- Mantener respuestas claras, precisas y amables.");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(contexto))
            {
                sb.AppendLine("Contexto del usuario:");
                sb.AppendLine(contexto);
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(ultimoMensaje))
            {
                sb.AppendLine("Último mensaje previo del usuario:");
                sb.AppendLine(ultimoMensaje);
                sb.AppendLine();
            }

            sb.AppendLine("Mensaje actual del usuario:");
            sb.AppendLine(mensajeNuevo);
            sb.AppendLine();
            sb.AppendLine("Responde:");

            return sb.ToString();
        }
    }
}
