using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using N5PermisosAPI.Models;

namespace CQRS.Commands
{
    public class CrearTipoPermisoCommand : IRequest<int>
    {
        public TipoPermiso TipoPermiso { get; set; }

        public CrearTipoPermisoCommand(TipoPermiso tipoPermiso)
        {
            TipoPermiso = tipoPermiso;
        }
    }
}
