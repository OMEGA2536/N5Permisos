using N5PermisosAPI.Models;
using MediatR;

namespace N5PermisosAPI.CQRS.Queries
{
    public class GetPermisosQuery : IRequest<IEnumerable<Permiso>>
    {
    }
}
