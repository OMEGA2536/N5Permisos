using MediatR;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.CQRS.Commands
{
    public class ModificarPermisoCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public Permiso Permiso { get; set; }

        public ModificarPermisoCommand(int id, Permiso permiso)
        {
            Id = id;
            Permiso = permiso;
        }
    }
}
