using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;

namespace Onboarding.Infrastructure.Repositories
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly IMongoCollection<Recurso> _collection;

        public RecursoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Recurso>("Recursos");
        }

        public async Task<List<Recurso>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Recurso?> GetByIdAsync(string id)
        {
            return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Recurso recurso)
        {
            await _collection.InsertOneAsync(recurso);
        }

        public async Task UpdateAsync(string id, Recurso recurso)
        {
            await _collection.ReplaceOneAsync(r => r.Id == id, recurso);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(r => r.Id == id);
        }
    }
}
