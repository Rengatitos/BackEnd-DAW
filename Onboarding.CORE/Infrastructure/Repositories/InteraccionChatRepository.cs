using MongoDB.Driver;
using Onboarding.CORE.Entities;
using Onboarding.CORE.Core.Interfaces;

namespace Onboarding.INFRA.Repositories
{
    public class InteraccionChatRepository : IInteraccionChatRepository
    {
        private readonly IMongoCollection<InteraccionChat> _collection;

        public InteraccionChatRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<InteraccionChat>("interacciones_chat"); //esta como minuscula
        }

        public async Task<List<InteraccionChat>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<List<InteraccionChat>> GetByUsuarioAsync(string usuarioRef)
        {
            return await _collection.Find(i => i.UsuarioRef == usuarioRef).ToListAsync();
        }

        public async Task<InteraccionChat?> GetByIdAsync(string id)
        {
            return await _collection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<InteraccionChat?> BuscarPorPreguntaAsync(string mensajeUsuario)
        {
            return await _collection
                .Find(i => i.MensajeUsuario.ToLower() == mensajeUsuario.ToLower())
                .SortByDescending(i => i.FechaHora)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(InteraccionChat interaccion)
        {
            await _collection.InsertOneAsync(interaccion);
        }

        public async Task UpdateAsync(InteraccionChat interaccion)
        {
            await _collection.ReplaceOneAsync(i => i.Id == interaccion.Id, interaccion);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(i => i.Id == id);
        }

        // Nuevo método (obtiene SOLO la última interacción del usuario)
        public async Task<InteraccionChat?> GetLastByUsuarioAsync(string usuarioRef)
        {
            return await _collection
                .Find(i => i.UsuarioRef == usuarioRef)
                .SortByDescending(i => i.FechaHora)
                .FirstOrDefaultAsync();
        }
    }
}
