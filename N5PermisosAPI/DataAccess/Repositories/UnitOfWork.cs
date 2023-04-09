using N5PermisosAPI.Data;
using N5PermisosAPI.DataAccess.Interfaces;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly N5PermisosAPIContext _context;
        private IPermisosRepository _permisos;
        public ITiposPermisoRepository TiposPermiso { get; private set; }

        public UnitOfWork(N5PermisosAPIContext context)
        {
            _context = context;
            TiposPermiso = new TiposPermisoRepository(context);
        }

        public IPermisosRepository Permisos => _permisos ??= new PermisosRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
