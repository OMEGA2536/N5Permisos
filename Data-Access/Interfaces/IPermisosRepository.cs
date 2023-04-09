using N5PermisosAPI.Models;

namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface IPermisosRepository : IRepository<Permiso>
    {
        Task<IEnumerable<Permiso>> GetAllAsync();
        Task CreateAsync(Permiso entity);
        void UpdateAsync(Permiso entity);
        bool PermisoExists(int id);
    }
}
