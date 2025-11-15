using Onboarding.CORE.Core.Interfaces;
using Onboarding.CORE.DTOs;
using Onboarding.CORE.Entities;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Onboarding.CORE.Services
{
    public class RecursoService : IRecursoService
    {
        private readonly IRecursoRepository _recursoRepository;

        public RecursoService(IRecursoRepository recursoRepository)
        {
            _recursoRepository = recursoRepository;
        }

        public async Task<List<RecursoDTO>> GetAllAsync()
        {
            var recursos = await _recursoRepository.GetAllAsync();
            return recursos.Select(r => new RecursoDTO
            {
                Id = r.Id!,
                Descripcion = r.Descripcion,
                Link = r.Link,
                Tipo = r.Tipo,
                Estado = r.Estado,
                FechaSubida = r.FechaSubida
            }).ToList();
        }

        public async Task<RecursoDTO?> GetByIdAsync(string id)
        {
            var recurso = await _recursoRepository.GetByIdAsync(id);
            if (recurso == null) return null;

            return new RecursoDTO
            {
                Id = recurso.Id!,
                Descripcion = recurso.Descripcion,
                Link = recurso.Link,
                Tipo = recurso.Tipo,
                Estado = recurso.Estado,
                FechaSubida = recurso.FechaSubida
            };
        }

        public async Task<RecursoDTO> CreateAsync(RecursoCreateDTO dto)
        {
            var recurso = new Recurso
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Descripcion = dto.Descripcion,
                Link = dto.Link,
                Tipo = dto.Tipo,
                AdminRef = dto.AdminRef,
                Estado = "Activo",
                FechaSubida = DateTime.UtcNow
            };

            await _recursoRepository.CreateAsync(recurso);

            return new RecursoDTO
            {
                Id = recurso.Id!,
                Descripcion = recurso.Descripcion,
                Link = recurso.Link,
                Tipo = recurso.Tipo,
                Estado = recurso.Estado,
                FechaSubida = recurso.FechaSubida
            };
        }

        public async Task<bool> UpdateAsync(string id, RecursoCreateDTO dto)
        {
            var recursoExistente = await _recursoRepository.GetByIdAsync(id);
            if (recursoExistente == null)
                return false;

            recursoExistente.Descripcion = dto.Descripcion;
            recursoExistente.Link = dto.Link;
            recursoExistente.Tipo = dto.Tipo;
            recursoExistente.AdminRef = dto.AdminRef;

            await _recursoRepository.UpdateAsync(id, recursoExistente);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var recurso = await _recursoRepository.GetByIdAsync(id);
            if (recurso == null)
                return false;

            await _recursoRepository.DeleteAsync(id);
            return true;
        }

        // Nuevos métodos
        public async Task<List<RecursoDTO>> GetByAdminAsync(string adminId)
        {
            var recursos = await _recursoRepository.GetByAdminRefAsync(adminId);
            return recursos.Select(r => new RecursoDTO
            {
                Id = r.Id!,
                Descripcion = r.Descripcion,
                Link = r.Link,
                Tipo = r.Tipo,
                Estado = r.Estado,
                FechaSubida = r.FechaSubida
            }).ToList();
        }

        public async Task<bool> UpdateEstadoAsync(string id, string estado)
        {
            var recursoExistente = await _recursoRepository.GetByIdAsync(id);
            if (recursoExistente == null)
                return false;

            return await _recursoRepository.UpdateEstadoAsync(id, estado);
        }

        public async Task<List<RecursoDTO>> GetByFechaRangeAsync(DateTime desde, DateTime hasta)
        {
            var recursos = await _recursoRepository.GetByFechaRangeAsync(desde, hasta);
            return recursos.Select(r => new RecursoDTO
            {
                Id = r.Id!,
                Descripcion = r.Descripcion,
                Link = r.Link,
                Tipo = r.Tipo,
                Estado = r.Estado,
                FechaSubida = r.FechaSubida
            }).ToList();
        }
    }
}
