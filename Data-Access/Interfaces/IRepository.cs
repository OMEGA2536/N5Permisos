namespace N5PermisosAPI.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task CreateAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        bool PermisoExists(int id);
    }
}

