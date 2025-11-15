using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;
using MongoDB.Bson;
using System;

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

        // Obtener recursos por admin_ref. Si adminId no es ObjectId válido devuelve lista vacía.
        public async Task<List<Recurso>> GetByAdminRefAsync(string adminId)
        {
            if (string.IsNullOrWhiteSpace(adminId))
                return new List<Recurso>();

            if (!ObjectId.TryParse(adminId, out var adminOid))
                return new List<Recurso>();

            var filter = Builders<Recurso>.Filter.Eq("admin_ref", adminOid);
            return await _collection.Find(filter).ToListAsync();
        }

        // Actualiza solo el campo 'estado'. Devuelve true si se modificó algún documento.
        public async Task<bool> UpdateEstadoAsync(string id, string estado)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (!ObjectId.TryParse(id, out var oid))
                return false;

            var filter = Builders<Recurso>.Filter.Eq("_id", oid);
            var update = Builders<Recurso>.Update.Set(r => r.Estado, estado);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        // Filtra por rango de fechas (inclusive).
        public async Task<List<Recurso>> GetByFechaRangeAsync(DateTime desde, DateTime hasta)
        {
            var desdeUtc = DateTime.SpecifyKind(desde, DateTimeKind.Utc);
            var hastaUtc = DateTime.SpecifyKind(hasta, DateTimeKind.Utc);

            var builder = Builders<Recurso>.Filter;
            var filter = builder.Gte(r => r.FechaSubida, desdeUtc) & builder.Lte(r => r.FechaSubida, hastaUtc);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
