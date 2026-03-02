using f_backend_gestafe.Data.Interfaces;
using f_backend_gestafe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace f_backend_gestafe.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task Update(T entity)
        {
            var entityId = _context.Entry(entity).Property("Id").CurrentValue;
            var trackedEntity = _context.ChangeTracker.Entries<T>()

                .FirstOrDefault(e => e.Property("Id").CurrentValue.Equals(entityId));

            if (trackedEntity != null)

            {

                _context.Entry(trackedEntity.Entity).State = EntityState.Detached;

            }

            _context.Entry(entity).State = EntityState.Modified;

            await SaveChanges();

        }
    }
}
