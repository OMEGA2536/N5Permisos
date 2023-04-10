using Microsoft.EntityFrameworkCore;
using N5PermisosAPI.Data;
using N5PermisosAPI.DataAccess.Interfaces;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly N5PermisosAPIContext _context;
        public Repository(N5PermisosAPIContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }

        public void UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}
