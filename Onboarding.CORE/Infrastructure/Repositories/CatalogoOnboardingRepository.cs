using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Linq;

namespace Onboarding.CORE.Infrastructure.Repositories
{
    public class CatalogoOnboardingRepository : ICatalogoOnboardingRepository
    {
        private readonly IMongoCollection<CatalogoOnboarding> _collection;

        public CatalogoOnboardingRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<CatalogoOnboarding>("CatalogoOnboarding");
        }

        public async Task<CatalogoOnboarding?> GetCatalogoAsync()
        {
            return await _collection.Find(_ => true).FirstOrDefaultAsync();
        }

        public async Task<CatalogoOnboarding?> GetEtapaAsync(string etapaNombre)
        {
            var catalogo = await GetCatalogoAsync();
            if (catalogo == null) return null;
            var etapa = catalogo.Etapas.FirstOrDefault(e => e.Nombre == etapaNombre);
            if (etapa == null) return null;
            return new CatalogoOnboarding { Id = catalogo.Id, Etapas = new System.Collections.Generic.List<EtapaOnboarding> { etapa } };
        }

        public async Task CreateCatalogoAsync(CatalogoOnboarding catalogo)
        {
            await _collection.InsertOneAsync(catalogo);
        }

        public async Task UpdateEtapaAsync(string etapaNombre, EtapaOnboarding etapa)
        {
            var filter = Builders<CatalogoOnboarding>.Filter.Eq(c => c.Id, "catalogo_onboarding") & Builders<CatalogoOnboarding>.Filter.Eq("etapas.nombre", etapaNombre);
            var update = Builders<CatalogoOnboarding>.Update.Set("etapas.$", etapa);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteEtapaAsync(string etapaNombre)
        {
            var filter = Builders<CatalogoOnboarding>.Filter.Eq(c => c.Id, "catalogo_onboarding");
            var update = Builders<CatalogoOnboarding>.Update.PullFilter(c => c.Etapas, e => e.Nombre == etapaNombre);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}