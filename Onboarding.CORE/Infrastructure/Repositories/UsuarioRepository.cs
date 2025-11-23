using MongoDB.Bson;
using MongoDB.Driver;
using Onboarding.CORE.Entities;
using Onboarding.CORE.Core.Interfaces;

namespace Onboarding.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Usuario>("Usuarios");
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _collection.Find(u => u.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _collection.Find(u => u.Correo == correo).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Usuario usuario)
        {
            await _collection.InsertOneAsync(usuario);
        }

        public async Task UpdateAsync(string id, Usuario usuario)
        {
            var objectId = ObjectId.Parse(id);
            usuario.Id = objectId; // importante
            await _collection.ReplaceOneAsync(u => u.Id == objectId, usuario);
        }

        public async Task DeleteAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _collection.DeleteOneAsync(u => u.Id == objectId);
        }

        public async Task<List<Usuario>> GetByRolRefAsync(string rolRef)
        {
            return await _collection.Find(u => u.RolRef == rolRef).ToListAsync();
        }
    }
}
