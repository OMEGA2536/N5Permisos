using MediatR;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.DataAccess.Repositories;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.CQRS.Handlers.CommandHandlers
{
    public class SolicitarPermisoCommandHandler : IRequestHandler<SolicitarPermisoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogEvent _logEvent;
        public SolicitarPermisoCommandHandler(IUnitOfWork unitOfWork, ILogEvent logEvent)
        {
            _unitOfWork = unitOfWork;
            _logEvent = logEvent;   

        }

        public async Task<int> Handle(SolicitarPermisoCommand request, CancellationToken cancellationToken)
        {
            var tipoPermiso = await _unitOfWork.TiposPermiso.GetByIdAsync(request.Permiso.TipoPermisoId);
            if (tipoPermiso == null)
            {
                throw new ArgumentException("Tipo de permiso no encontrado");
            }

            request.Permiso.TipoPermiso = tipoPermiso;
            await _unitOfWork.Permisos.CreateAsync(request.Permiso);
            await _unitOfWork.SaveAsync();
            await _logEvent.LogEventToElasticsearchAsync("SolicitarPermiso", request.Permiso);
            await _logEvent.LogEventToKafkaAsync("request", request.Permiso);
            return request.Permiso.Id;
        }
    }
}
