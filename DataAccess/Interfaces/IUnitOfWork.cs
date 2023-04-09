namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermisosRepository Permisos { get; }
        Task<int> SaveAsync();
    }
}
