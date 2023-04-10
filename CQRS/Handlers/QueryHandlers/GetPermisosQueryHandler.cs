using MediatR;
using N5PermisosAPI.CQRS.Queries;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.DataAccess.Repositories;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.CQRS.Handlers.QueryHandlers
{
    public class GetPermisosQueryHandler : IRequestHandler<GetPermisosQuery, IEnumerable<Permiso>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogEvent _logEvent;
        public GetPermisosQueryHandler(IUnitOfWork unitOfWork, ILogEvent logEvent)
        {
            _unitOfWork = unitOfWork;
            _logEvent = logEvent;
        }
        public async Task<IEnumerable<Permiso>> Handle(GetPermisosQuery request, CancellationToken cancellationToken)
        {
            var permisos = await _unitOfWork.Permisos.GetAllAsync();
            await _logEvent.LogEventToElasticsearchAsync("GetPermisos", permisos);
            await _logEvent.LogEventToKafkaAsync("get", permisos);
            return permisos;
        }

    }
}
