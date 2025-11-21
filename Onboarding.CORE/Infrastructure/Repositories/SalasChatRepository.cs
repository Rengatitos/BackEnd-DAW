using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Onboarding.CORE.Infrastructure.Repositories
{
    public class SalasChatRepository : ISalasChatRepository
    {
        private readonly IMongoCollection<SalaChat> _collection;

        public SalasChatRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<SalaChat>("SalasChat");
        }

        public async Task<SalaChat?> GetByUsuarioRefAsync(string usuarioRef)
        {
            var filter = Builders<SalaChat>.Filter.Eq(s => s.UsuarioRef, usuarioRef);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(SalaChat sala)
        {
            await _collection.InsertOneAsync(sala);
        }

        public async Task UpdateAsync(SalaChat sala)
        {
            var filter = Builders<SalaChat>.Filter.Eq(s => s.Id, sala.Id);
            await _collection.ReplaceOneAsync(filter, sala);
        }

        public async Task DeleteAsync(string usuarioRef)
        {
            var filter = Builders<SalaChat>.Filter.Eq(s => s.UsuarioRef, usuarioRef);
            await _collection.DeleteOneAsync(filter);
        }
    }
}