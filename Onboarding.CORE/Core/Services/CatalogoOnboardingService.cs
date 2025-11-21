using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Onboarding.CORE.Services
{
    public class CatalogoOnboardingService : ICatalogoOnboardingService
    {
        private readonly ICatalogoOnboardingRepository _repo;

        public CatalogoOnboardingService(ICatalogoOnboardingRepository repo)
        {
            _repo = repo;
        }

        public async Task<CatalogoOnboardingDTO?> GetCatalogoAsync()
        {
            var c = await _repo.GetCatalogoAsync();
            if (c == null) return null;
            return new CatalogoOnboardingDTO
            {
                Id = c.Id,
                Etapas = c.Etapas.Select(e => new EtapaOnboardingDTO
                {
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Documentos = e.Documentos,
                    Urls = e.Urls,
                    ProximosPasos = e.ProximosPasos,
                    Consejos = e.Consejos
                }).ToList()
            };
        }

        public async Task<EtapaOnboardingDTO?> GetEtapaAsync(string etapaNombre)
        {
            var e = await _repo.GetEtapaAsync(etapaNombre);
            if (e == null) return null;
            return new EtapaOnboardingDTO
            {
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Documentos = e.Documentos,
                Urls = e.Urls,
                ProximosPasos = e.ProximosPasos,
                Consejos = e.Consejos
            };
        }

        public async Task CreateCatalogoAsync(CatalogoOnboardingCreateDTO dto)
        {
            var existing = await _repo.GetCatalogoAsync();
            if (existing != null) throw new System.Exception("Catalogo ya existe");

            var catalogo = new CatalogoOnboarding
            {
                Id = "catalogo_onboarding",
                Etapas = dto.Etapas.Select(e => new EtapaOnboarding
                {
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    Documentos = e.Documentos,
                    Urls = e.Urls,
                    ProximosPasos = e.ProximosPasos,
                    Consejos = e.Consejos
                }).ToList()
            };

            await _repo.CreateCatalogoAsync(catalogo);
        }

        public async Task<bool> UpdateEtapaAsync(string etapaNombre, CatalogoOnboardingUpdateEtapaDTO dto)
        {
            var existing = await _repo.GetCatalogoAsync();
            if (existing == null) return false;
            var etapa = existing.Etapas.FirstOrDefault(x => x.Nombre == etapaNombre);
            if (etapa == null) return false;

            var nueva = new EtapaOnboarding
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Documentos = dto.Documentos,
                Urls = dto.Urls,
                ProximosPasos = dto.ProximosPasos,
                Consejos = dto.Consejos
            };

            await _repo.UpdateEtapaAsync(etapaNombre, nueva);
            return true;
        }

        public async Task<bool> DeleteEtapaAsync(string etapaNombre)
        {
            var existing = await _repo.GetCatalogoAsync();
            if (existing == null) return false;
            var etapa = existing.Etapas.FirstOrDefault(x => x.Nombre == etapaNombre);
            if (etapa == null) return false;
            await _repo.DeleteEtapaAsync(etapaNombre);
            return true;
        }
    }
}