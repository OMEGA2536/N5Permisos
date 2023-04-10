using N5PermisosAPI.Models;

namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface ITiposPermisoRepository : IRepository<TipoPermiso>
    {
        Task CreateAsync(TipoPermiso tipoPermiso);
    }
}
