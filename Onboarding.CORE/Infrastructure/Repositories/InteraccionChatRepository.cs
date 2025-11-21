using MongoDB.Driver;
using Onboarding.CORE.Entities;
using Onboarding.CORE.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Onboarding.INFRA.Repositories
{
    public class InteraccionChatRepository : IInteraccionChatRepository
    {
        private readonly IMongoCollection<InteraccionChat> _collection;

        public InteraccionChatRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<InteraccionChat>("interacciones_chat");
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
            if (string.IsNullOrWhiteSpace(mensajeUsuario)) return null;

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

        public async Task<InteraccionChat?> GetLastByUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef)) return null;

            return await _collection
                .Find(i => i.UsuarioRef == usuarioRef)
                .SortByDescending(i => i.FechaHora)
                .FirstOrDefaultAsync();
        }
    }
}
