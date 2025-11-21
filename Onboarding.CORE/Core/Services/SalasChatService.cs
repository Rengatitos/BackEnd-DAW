using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;
using System.Threading.Tasks;
using System;

namespace Onboarding.CORE.Services
{
    public class SalasChatService : ISalasChatService
    {
        private readonly ISalasChatRepository _repo;

        public SalasChatService(ISalasChatRepository repo)
        {
            _repo = repo;
        }

        public async Task<SalaChatDTO?> GetByUsuarioRefAsync(string usuarioRef)
        {
            var s = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (s == null) return null;
            return MapToDTO(s);
        }

        public async Task<SalaChatDTO> CreateAsync(SalaChatCreateDTO dto)
        {
            var existing = await _repo.GetByUsuarioRefAsync(dto.UsuarioRef);
            if (existing != null) throw new Exception("Sala ya existe para este usuario");

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
                EstadoOnboardingIA = new EstadoOnboardingIA(),
                ContextoPersistente = string.Empty,
                UltimoMensaje = string.Empty,
                UltimaActualizacion = DateTime.UtcNow
            };

            await _repo.CreateAsync(sala);
            return MapToDTO(sala);
        }

        public async Task<bool> UpdateEstadoAsync(string usuarioRef, SalaChatUpdateEstadoDTO dto)
        {
            var existing = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (existing == null) return false;

            existing.NivelOnboarding = new NivelOnboarding
            {
                Etapa = dto.NivelOnboarding.Etapa,
                Porcentaje = dto.NivelOnboarding.Porcentaje,
                UltimaActualizacion = dto.NivelOnboarding.UltimaActualizacion,
                Estado = dto.NivelOnboarding.Estado
            };

            existing.EstadoOnboardingIA = new EstadoOnboardingIA
            {
                PasoActual = dto.EstadoOnboardingIA.PasoActual,
                HaVistoDocumentos = dto.EstadoOnboardingIA.HaVistoDocumentos,
                HaConsultadoSupervisor = dto.EstadoOnboardingIA.HaConsultadoSupervisor,
                HaSolicitadoAdmin = dto.EstadoOnboardingIA.HaSolicitadoAdmin
            };

            existing.UltimoMensaje = dto.UltimoMensaje;
            existing.UltimaActualizacion = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> UpdateContextoAsync(string usuarioRef, SalaChatUpdateContextoDTO dto)
        {
            var existing = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (existing == null) return false;

            existing.ContextoPersistente = dto.ContextoPersistente;
            existing.UltimoMensaje = dto.UltimoMensaje;
            existing.UltimaActualizacion = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(string usuarioRef)
        {
            var existing = await _repo.GetByUsuarioRefAsync(usuarioRef);
            if (existing == null) return false;
            await _repo.DeleteAsync(usuarioRef);
            return true;
        }

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
    }
}