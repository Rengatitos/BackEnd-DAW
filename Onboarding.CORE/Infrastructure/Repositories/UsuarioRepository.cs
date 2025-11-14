using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;

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
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
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
            await _collection.ReplaceOneAsync(u => u.Id == id, usuario);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
