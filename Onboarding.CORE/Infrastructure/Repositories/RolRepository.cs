using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;

namespace Onboarding.Infrastructure.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly IMongoCollection<Rol> _collection;

        public RolRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Rol>("Roles");
        }

        public async Task<List<Rol>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(string id)
        {
            return await _collection.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            return await _collection.Find(r => r.Nombre == nombre).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Rol rol)
        {
            await _collection.InsertOneAsync(rol);
        }

        public async Task UpdateAsync(string id, Rol rol)
        {
            await _collection.ReplaceOneAsync(r => r.Id == id, rol);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(r => r.Id == id);
        }
    }
}
