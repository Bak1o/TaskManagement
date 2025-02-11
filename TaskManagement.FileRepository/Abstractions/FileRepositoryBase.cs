using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models.Abstraction;

namespace TaskManagement.FileRepository.Abstractions
{
    public abstract class FileRepositoryBase<TEntity, TId>
        where TEntity : DomainEntity<TId>
        where TId : IComparable<TId>

    {
        private readonly string _filePath;
        private readonly List<TEntity> _entities;
        public FileRepositoryBase(string filePath)
        {
            _filePath = filePath;
            _entities = LoadEntitiesFromFile();


        }

        protected abstract Task<TId> GenerateIdAsync();

        public List<TEntity> LoadEntitiesFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TEntity>();
            }

            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<TEntity>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<TEntity>();
        }
        public async Task<TId> CreateAsync(TEntity entityToCreate)
        {
            if (entityToCreate.Id == null)
            {
                throw new ArgumentNullException(nameof(entityToCreate.Id));
            }
            if (_entities.Any(x => x.Id.CompareTo(entityToCreate.Id) == 0))
            {
                throw new ArgumentException($"Entity with id {entityToCreate.Id} already exists");
            }
           await SaveAsync(entityToCreate);
            return entityToCreate.Id;
            
        }

        public Task SaveAsync(TEntity entityToSave)
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(entityToSave));
            return Task.CompletedTask;
        }
        public Task DeleteAsync(TId id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var entityToDelete = _entities.Find(x => x.Id.Equals(id));
            if (entityToDelete is null)
            {
                throw new ObjectNotFoundException(id.ToString() ?? string.Empty, nameof(TEntity));
            }
            _entities.Remove(entityToDelete);
            SaveChangesAsync();

            return Task.CompletedTask;
        }

        public Task<TEntity> GetByIdOrDefaultAsync(TId id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var entityExists = _entities.FirstOrDefault(x => x.Id.CompareTo(id) == 0);
            if (entityExists is null)
            {
                throw new ObjectNotFoundException(id.ToString() ?? string.Empty, nameof(TEntity));
            }
            return Task.FromResult(entityExists);




        }

        public Task<TEntity> GetByIdAsync(TId id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var entityExists = _entities.FirstOrDefault(x => x.Id.CompareTo(id) == 0);
            if (entityExists is null)
            {
                throw new ObjectNotFoundException(id.ToString() ?? string.Empty, nameof(TEntity));
            }
            return Task.FromResult(entityExists);
        }





        public Task UpdateAsync(TEntity entityToUpdate)
        {
            if (entityToUpdate.Id == null)
            {
                throw new ArgumentNullException(nameof(entityToUpdate.Id));
            }
            var entityExists = _entities.FirstOrDefault(x => x.Id.CompareTo(entityToUpdate.Id) == 0);
            if (entityExists is null)
            {
                throw new ObjectNotFoundException(entityToUpdate.Id.ToString() ?? string.Empty, nameof(TEntity));
            }
            entityExists = entityToUpdate;
            SaveChangesAsync();
            return Task.CompletedTask;


        }

        public Task<List<TEntity>> ListAsync()
        {
            return Task.FromResult(_entities);

        }
        public Task SaveChangesAsync()
        {
            File.WriteAllText(_filePath, string.Empty);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_entities));
            return Task.CompletedTask;
        }
    }
}
