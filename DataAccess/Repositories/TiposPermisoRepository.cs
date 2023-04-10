using N5PermisosAPI.Data;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.DataAccess.Repositories
{
    public class TiposPermisoRepository : Repository<TipoPermiso>, ITiposPermisoRepository
    {
        public TiposPermisoRepository(N5PermisosAPIContext context) : base(context)
        {
        }
    }
}
