using MediatR;
using N5PermisosAPI.Models;
using System.Windows.Input;

namespace N5PermisosAPI.CQRS.Commands
{
    public class SolicitarPermisoCommand : IRequest<int>
    {
        public Permiso Permiso { get; set; }

        public SolicitarPermisoCommand(Permiso permiso)
        {
            Permiso = permiso;
        }
    }
}
