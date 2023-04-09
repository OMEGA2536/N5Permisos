namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermisosRepository Permisos { get; }
        ITiposPermisoRepository TiposPermiso { get; }
        Task<int> SaveAsync();
    }
}
