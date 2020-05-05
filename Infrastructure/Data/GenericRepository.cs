using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        private DbSet<T> _entities;
        public GenericRepository(StoreContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetEntityBySpecAsync(ISpecification<T> spec)
        {
            return await GetQueryAfterApplyingSpec(spec).FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllBySpecAsync(ISpecification<T> spec)
        {
            return await GetQueryAfterApplyingSpec(spec).ToListAsync();
        }
        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await GetQueryAfterApplyingSpec(spec).CountAsync();
        }
        private IQueryable<T> GetQueryAfterApplyingSpec(ISpecification<T> spec)
        {
            return QueryBuilder<T>.GetQuery(_entities.AsQueryable(), spec);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            _entities.Attach(entity);
            _context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }
    }
}