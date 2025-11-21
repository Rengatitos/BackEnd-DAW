using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;
using Onboarding.CORE.Helpers;
using MongoDB.Bson;

namespace Onboarding.CORE.Services
{
    public class InteraccionChatService : IInteraccionChatService
    {
        private readonly IInteraccionChatRepository _interaccionRepository;
        private readonly OllamaClient _ollamaClient;

        // Inyectar OllamaClient via DI
        public InteraccionChatService(IInteraccionChatRepository interaccionRepository, OllamaClient ollamaClient)
        {
            _interaccionRepository = interaccionRepository;
            _ollamaClient = ollamaClient;
        }

        public async Task<List<InteraccionChatDTO>> GetAllAsync()
        {
            var interacciones = await _interaccionRepository.GetAllAsync();
            return interacciones.Select(MapToDTO).ToList();
        }

        public async Task<List<InteraccionChatDTO>> GetByUsuarioAsync(string usuarioRef)
        {
            var interacciones = await _interaccionRepository.GetByUsuarioAsync(usuarioRef);
            return interacciones.Select(MapToDTO).ToList();
        }

        public async Task<InteraccionChatDTO?> GetByIdAsync(string id)
        {
            var interaccion = await _interaccionRepository.GetByIdAsync(id);
            return interaccion == null ? null : MapToDTO(interaccion);
        }

        public async Task<InteraccionChatDTO> GenerarInteraccionAsync(InteraccionChatCreateDTO dto)
        {
            var historico = await _interaccionRepository.BuscarPorPreguntaAsync(dto.MensajeUsuario);
            string respuestaFinal;

            if (historico != null && historico.EsCorregida && !string.IsNullOrEmpty(historico.RespuestaCorregida))
            {
                respuestaFinal = historico.RespuestaCorregida!;
            }
            else
            {
                var prompt = $"{dto.MensajeUsuario}\nPor favor responde de forma clara y breve.";
                respuestaFinal = await _ollamaClient.GenerarRespuestaAsync(prompt);
                if (string.IsNullOrWhiteSpace(respuestaFinal))
                    respuestaFinal = "⚠️ No se pudo generar respuesta. Intenta nuevamente.";
            }

            var interaccion = new InteraccionChat
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UsuarioRef = dto.UsuarioRef,
                MensajeUsuario = dto.MensajeUsuario,
                RespuestaChatbot = respuestaFinal,
                FechaHora = DateTime.UtcNow,
                Contexto = dto.Contexto
            };

            await _interaccionRepository.CreateAsync(interaccion);

            Console.WriteLine($"✅ Interacción guardada en MongoDB para usuario {dto.UsuarioRef}");

            return MapToDTO(interaccion);
        }

        public async Task CreateAsync(InteraccionChatCreateDTO dto)
        {
            Console.WriteLine($"🟢 Creando interacción para usuario: {dto.UsuarioRef}");

            var interaccion = new InteraccionChat
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UsuarioRef = dto.UsuarioRef,
                MensajeUsuario = dto.MensajeUsuario,
                RespuestaChatbot = dto.RespuestaChatbot,
                FechaHora = DateTime.UtcNow,
                Contexto = dto.Contexto
            };

            await _interaccionRepository.CreateAsync(interaccion);

            Console.WriteLine("✅ Interacción enviada al repositorio correctamente.");
        }

        public async Task CorregirRespuestaAsync(string id, string nuevaRespuesta)
        {
            var interaccion = await _interaccionRepository.GetByIdAsync(id);
            if (interaccion == null)
                throw new Exception("Interacción no encontrada.");

            interaccion.RespuestaCorregida = nuevaRespuesta;
            interaccion.EsCorregida = true;

            await _interaccionRepository.UpdateAsync(interaccion);
        }

        public async Task DeleteAsync(string id)
        {
            await _interaccionRepository.DeleteAsync(id);
        }

        // Nuevo método: obtiene solo la última interacción del usuario
        public async Task<InteraccionChatDTO?> GetLastByUsuarioAsync(string usuarioRef)
        {
            var interaccion = await _interaccionRepository.GetLastByUsuarioAsync(usuarioRef);
            return interaccion == null ? null : MapToDTO(interaccion);
        }

        private static InteraccionChatDTO MapToDTO(InteraccionChat i)
        {
            return new InteraccionChatDTO
            {
                Id = i.Id!,
                UsuarioRef = i.UsuarioRef,
                MensajeUsuario = i.MensajeUsuario,
                RespuestaChatbot = i.RespuestaChatbot,
                RespuestaCorregida = i.RespuestaCorregida,
                EsCorregida = i.EsCorregida,
                FechaHora = i.FechaHora,
                Contexto = i.Contexto
            };
        }
    }
}
