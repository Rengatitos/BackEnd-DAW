using Onboarding.CORE.Core.DTOs;
using Onboarding.CORE.Core.Entities;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;
using System;
using System.Threading.Tasks;
using SalaChatUpdateEstadoDTO = Onboarding.CORE.DTOs.SalaChatUpdateEstadoDTO;

namespace Onboarding.CORE.Services
{
    public class SalasChatService : ISalasChatService
    {
        private readonly ISalasChatRepository _repo;

        public SalasChatService(ISalasChatRepository repo)
        {
            _repo = repo;
        }

        // ============================================================
        // GET – Obtener sala del usuario
        // ============================================================
        public async Task<SalaChatDTO?> GetByUsuarioRefAsync(string usuarioRef)
        {
            var entity = await _repo.GetByUsuarioRefAsync(usuarioRef);
            return entity == null ? null : MapToDTO(entity);
        }

        // ============================================================
        // CREATE – Crear una sala de chat
        // ============================================================
        public async Task<SalaChatDTO> CreateAsync(SalaChatCreateDTO dto)
        {
            var existing = await _repo.GetByUsuarioRefAsync(dto.UsuarioRef);
            if (existing != null)
                throw new Exception("La sala para este usuario ya existe.");

            var sala = new SalaChat
            {
                Id = $"sala_{dto.UsuarioRef}",
                UsuarioRef = dto.UsuarioRef,
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Area = dto.Area,
                RolRef = dto.RolRef,

                NivelOnboarding = new NivelOnboarding
                {
                    Etapa = "Inicial",
                    Porcentaje = 0,
                    UltimaActualizacion = DateTime.UtcNow,
                    Estado = "Activo"
                },

                EstadoOnboardingIA = new EstadoOnboardingIA
                {
                    PasoActual = "Inicial",
                    HaVistoDocumentos = false,
                    HaConsultadoSupervisor = false,
                    HaSolicitadoAdmin = false
                },

                ContextoPersistente = "",
                UltimoMensaje = "",
                UltimaActualizacion = DateTime.UtcNow
            };

            await _repo.CreateAsync(sala);
            return MapToDTO(sala);
        }

        // ============================================================
        // UPDATE – Estado del onboarding (IA + nivel)
        // ============================================================
        public async Task<bool> UpdateEstadoAsync(string usuarioRef, SalaChatUpdateEstadoDTO dto)
        {
            var entity = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (entity == null) return false;

            entity.NivelOnboarding = new NivelOnboarding
            {
                Etapa = dto.NivelOnboarding.Etapa,
                Porcentaje = dto.NivelOnboarding.Porcentaje,
                UltimaActualizacion = DateTime.UtcNow,
                Estado = dto.NivelOnboarding.Estado
            };

            entity.EstadoOnboardingIA = new EstadoOnboardingIA
            {
                PasoActual = dto.EstadoOnboardingIA.PasoActual,
                HaVistoDocumentos = dto.EstadoOnboardingIA.HaVistoDocumentos,
                HaConsultadoSupervisor = dto.EstadoOnboardingIA.HaConsultadoSupervisor,
                HaSolicitadoAdmin = dto.EstadoOnboardingIA.HaSolicitadoAdmin
            };

            entity.UltimoMensaje = dto.UltimoMensaje;
            entity.UltimaActualizacion = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            return true;
        }

        // ============================================================
        // UPDATE – Sólo actualizar el contexto persistente
        // ============================================================
        public async Task<bool> UpdateContextoAsync(string usuarioRef, SalaChatUpdateContextoDTO dto)
        {
            var entity = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (entity == null) return false;

            entity.ContextoPersistente = dto.ContextoPersistente;
            entity.UltimoMensaje = dto.UltimoMensaje;
            entity.UltimaActualizacion = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            return true;
        }

        // ============================================================
        // DELETE – Eliminar sala
        // ============================================================
        public async Task<bool> DeleteAsync(string usuarioRef)
        {
            var existing = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (existing == null) return false;

            await _repo.DeleteAsync(usuarioRef);
            return true;
        }

        // ============================================================
        // MAP – Convertir Entity → DTO
        // ============================================================
        private static SalaChatDTO MapToDTO(SalaChat s)
        {
            return new SalaChatDTO
            {
                Id = s.Id,
                UsuarioRef = s.UsuarioRef,
                Nombre = s.Nombre,
                Correo = s.Correo,
                Area = s.Area,
                RolRef = s.RolRef,

                NivelOnboarding = new NivelOnboardingDTO
                {
                    Etapa = s.NivelOnboarding.Etapa,
                    Porcentaje = s.NivelOnboarding.Porcentaje,
                    UltimaActualizacion = s.NivelOnboarding.UltimaActualizacion,
                    Estado = s.NivelOnboarding.Estado
                },

                EstadoOnboardingIA = new EstadoOnboardingIADTO
                {
                    PasoActual = s.EstadoOnboardingIA.PasoActual,
                    HaVistoDocumentos = s.EstadoOnboardingIA.HaVistoDocumentos,
                    HaConsultadoSupervisor = s.EstadoOnboardingIA.HaConsultadoSupervisor,
                    HaSolicitadoAdmin = s.EstadoOnboardingIA.HaSolicitadoAdmin
                },

                ContextoPersistente = s.ContextoPersistente,
                UltimoMensaje = s.UltimoMensaje,
                UltimaActualizacion = s.UltimaActualizacion
            };
        }

        public Task<bool> UpdateEstadoAsync(string usuarioRef, Core.Interfaces.SalaChatUpdateEstadoDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
