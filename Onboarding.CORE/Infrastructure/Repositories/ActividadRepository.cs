using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.Entities;

namespace Onboarding.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio para gestionar operaciones de Actividades en MongoDB
    /// </summary>
    public class ActividadRepository : IActividadRepository
    {
        private readonly IMongoCollection<Actividad> _collection;

        public ActividadRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Actividad>("Actividades");
        }

        /// <summary>
        /// Obtiene todas las actividades
        /// </summary>
        public async Task<List<Actividad>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Obtiene actividades por referencia de usuario
        /// </summary>
        public async Task<List<Actividad>> GetByUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef))
                return new List<Actividad>();

            return await _collection.Find(a => a.UsuarioRef == usuarioRef).ToListAsync();
        }

        /// <summary>
        /// Obtiene una actividad por su ID
        /// </summary>
        public async Task<Actividad?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _collection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Crea una nueva actividad
        /// </summary>
        public async Task<Actividad> CreateAsync(Actividad actividad)
        {
            if (actividad == null)
                throw new ArgumentNullException(nameof(actividad));

            await _collection.InsertOneAsync(actividad);
            return actividad; // CAMBIO: Retorna con el ID generado
        }

        /// <summary>
        /// Actualiza una actividad existente
        /// </summary>
        public async Task<bool> UpdateAsync(string id, Actividad actividad)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (actividad == null)
                throw new ArgumentNullException(nameof(actividad));

            var result = await _collection.ReplaceOneAsync(a => a.Id == id, actividad);
            return result.ModifiedCount > 0; // CAMBIO: Retorna si realmente se modificó
        }

        /// <summary>
        /// Elimina una actividad por su ID
        /// </summary>
        public async Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var result = await _collection.DeleteOneAsync(a => a.Id == id);
            return result.DeletedCount > 0; // CAMBIO: Retorna si realmente se eliminó
        }

        /// <summary>
        /// Obtiene actividades por estado
        /// </summary>
        public async Task<List<Actividad>> GetByEstadoAsync(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return new List<Actividad>();

            return await _collection.Find(a => a.Estado == estado).ToListAsync();
        }

        /// <summary>
        /// Obtiene actividades en un rango de fechas
        /// </summary>
        public async Task<List<Actividad>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _collection.Find(a =>
                a.FechaInicio >= fechaInicio && a.FechaFin <= fechaFin
            ).ToListAsync();
        }

        /// <summary>
        /// Actualiza solo el estado de una actividad
        /// </summary>
        public async Task<bool> UpdateEstadoAsync(string id, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nuevoEstado))
                return false;

            var filter = Builders<Actividad>.Filter.Eq(a => a.Id, id);
            var update = Builders<Actividad>.Update.Set(a => a.Estado, nuevoEstado);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Verifica si existe una actividad por ID
        /// </summary>
        public async Task<bool> ExistsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var count = await _collection.CountDocumentsAsync(a => a.Id == id);
            return count > 0;
        }

        /// <summary>
        /// Obtiene el conteo de actividades de un usuario específico
        /// </summary>
        public async Task<long> GetCountByUsuarioAsync(string usuarioRef)
        {
            if (string.IsNullOrWhiteSpace(usuarioRef))
                return 0;

            return await _collection.CountDocumentsAsync(a => a.UsuarioRef == usuarioRef);
        }

    }
}