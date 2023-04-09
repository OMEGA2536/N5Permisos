using Microsoft.EntityFrameworkCore;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.Data
{
    public class N5PermisosAPIContext : DbContext
    {
        public N5PermisosAPIContext(DbContextOptions<N5PermisosAPIContext> options)
           : base(options)
        {
        }

        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<TipoPermiso> TiposPermiso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permiso>()
                .HasOne(p => p.TipoPermiso)
                .WithMany()
                .HasForeignKey(p => p.TipoPermisoId);
        }
    }
}
