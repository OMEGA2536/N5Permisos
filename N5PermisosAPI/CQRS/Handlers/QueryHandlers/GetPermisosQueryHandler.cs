using MediatR;
using N5PermisosAPI.CQRS.Queries;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.CQRS.Handlers.QueryHandlers
{
    public class GetPermisosQueryHandler : IRequestHandler<GetPermisosQuery, IEnumerable<Permiso>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPermisosQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Permiso>> Handle(GetPermisosQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Permisos.GetAllAsync();
        }

    }
}
