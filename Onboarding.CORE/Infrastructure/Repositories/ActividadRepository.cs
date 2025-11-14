using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;

namespace Onboarding.Infrastructure.Repositories
{
    public class ActividadRepository : IActividadRepository
    {
        private readonly IMongoCollection<Actividad> _collection;

        public ActividadRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Actividad>("Actividades");
        }

        public async Task<List<Actividad>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Actividad>> GetByUsuarioAsync(string usuarioRef)
        {
            return await _collection.Find(a => a.UsuarioRef == usuarioRef).ToListAsync();
        }

        public async Task<Actividad?> GetByIdAsync(string id)
        {
            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Actividad actividad)
        {
            await _collection.InsertOneAsync(actividad);
        }

        public async Task UpdateAsync(string id, Actividad actividad)
        {
            await _collection.ReplaceOneAsync(a => a.Id == id, actividad);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(a => a.Id == id);
        }
    }
}
