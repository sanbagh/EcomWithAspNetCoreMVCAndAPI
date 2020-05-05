using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;

        }
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type.Name))
            {
                var repo = Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(type), _context);
                _repositories.Add(type.Name, repo);
            }
            return (IGenericRepository<TEntity>)_repositories[type.Name];
        }
    }
}