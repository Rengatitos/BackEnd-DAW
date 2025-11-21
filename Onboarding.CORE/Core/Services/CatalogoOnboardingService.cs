using Onboarding.CORE.Core.Entities;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Core.DTOs;
using Onboarding.CORE.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Onboarding.CORE.DTOs;

namespace Onboarding.CORE.Services
{
    public class CatalogoOnboardingService : ICatalogoOnboardingService
    {
        private readonly ICatalogoOnboardingRepository _repo;

        public CatalogoOnboardingService(ICatalogoOnboardingRepository repo)
        {
            _repo = repo;
        }

        // ============================
        //      OBTENER CATÁLOGO
        // ============================
        public async Task<CatalogoOnboardingDTO?> GetCatalogoAsync()
        {
            var catalogo = await _repo.GetCatalogoAsync();
            if (catalogo == null) return null;

            return new CatalogoOnboardingDTO
            {
                Id = catalogo.Id,
                Etapas = catalogo.Etapas.Select(MapToDTO).ToList()
            };
        }

        // ============================
        //      OBTENER UNA ETAPA
        // ============================
        public async Task<DTOs.EtapaOnboardingDTO?> GetEtapaAsync(string etapaNombre)
        {
            var etapa = await _repo.GetEtapaAsync(etapaNombre);
            if (etapa == null) return null;

            return MapToDTO(etapa);
        }

        // ============================
        //      CREAR CATÁLOGO
        // ============================
        public async Task CreateCatalogoAsync(DTOs.CatalogoOnboardingCreateItemDTO dto)
        {
            var existing = await _repo.GetCatalogoAsync();
            if (existing != null)
                throw new System.Exception("❌ Ya existe un catálogo creado. Solo puede existir uno.");

            var catalogo = new CatalogoOnboarding
            {
                Id = "catalogo_onboarding",
                Etapas = dto.Etapas.Select(MapToEntity).ToList()
            };

            await _repo.CreateCatalogoAsync(catalogo);
        }

        // ============================
        //     ACTUALIZAR UNA ETAPA
        // ============================
        public async Task<bool> UpdateEtapaAsync(string etapaNombre, DTOs.CatalogoOnboardingUpdateEtapaDTO dto)
        {
            var catalogo = await _repo.GetCatalogoAsync();
            if (catalogo == null) return false;

            var etapaExistente = catalogo.Etapas.FirstOrDefault(e => e.Nombre == etapaNombre);
            if (etapaExistente == null) return false;

            var nuevaEtapa = new EtapaOnboarding
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Documentos = dto.Documentos,
                Urls = dto.Urls,
                ProximosPasos = dto.ProximosPasos,
                Consejos = dto.Consejos
            };

            await _repo.UpdateEtapaAsync(etapaNombre, nuevaEtapa);
            return true;
        }

        // ============================
        //     ELIMINAR UNA ETAPA
        // ============================
        public async Task<bool> DeleteEtapaAsync(string etapaNombre)
        {
            var catalogo = await _repo.GetCatalogoAsync();
            if (catalogo == null) return false;

            var etapa = catalogo.Etapas.FirstOrDefault(e => e.Nombre == etapaNombre);
            if (etapa == null) return false;

            await _repo.DeleteEtapaAsync(etapaNombre);
            return true;
        }

        // ============================
        //         MAPEO DTO
        // ============================
        private DTOs.EtapaOnboardingDTO MapToDTO(EtapaOnboarding e)
        {
            return new DTOs.EtapaOnboardingDTO
            {
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Documentos = e.Documentos ?? new List<string>(),
                Urls = e.Urls ?? new List<string>(),
                ProximosPasos = e.ProximosPasos ?? new List<string>(),
                Consejos = e.Consejos ?? new List<string>()
            };
        }

        private EtapaOnboarding MapToEntity(DTOs.EtapaOnboardingDTO e)
        {
            return new EtapaOnboarding
            {
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Documentos = e.Documentos ?? new List<string>(),
                Urls = e.Urls ?? new List<string>(),
                ProximosPasos = e.ProximosPasos ?? new List<string>(),
                Consejos = e.Consejos ?? new List<string>()
            };
        }
    }
}
