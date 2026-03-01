using Microsoft.EntityFrameworkCore;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Context;

namespace SGC.Persistence.Base
{
    /// Todos los repositorios específicos heredan de esta clase.
    
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SGCDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(SGCDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

      
        // Expone el contexto a los repositorios hijos para consultas personalizadas.
      
        protected SGCDbContext Context => _context;

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"No se encontró la entidad con Id {id}.");
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
