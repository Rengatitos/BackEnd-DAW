using MongoDB.Driver;
using Onboarding.CORE.Entities;
using Onboarding.CORE.Core.Interfaces;
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
            return await _collection.Find(s => s.UsuarioRef == usuarioRef).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(SalaChat sala)
        {
            await _collection.InsertOneAsync(sala);
        }

        public async Task UpdateAsync(SalaChat sala)
        {
            await _collection.ReplaceOneAsync(s => s.Id == sala.Id, sala);
        }

        public async Task DeleteAsync(string usuarioRef)
        {
            await _collection.DeleteOneAsync(s => s.UsuarioRef == usuarioRef);
        }

        public Task<bool> UpdateEstadoAsync(string usuarioRef, string nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateContextoAsync(string usuarioRef, string nuevoContexto)
        {
            throw new NotImplementedException();
        }
    }
}
