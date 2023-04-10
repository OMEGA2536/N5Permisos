using MediatR;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.DataAccess.Repositories;
using N5PermisosAPI.Models;

namespace N5PermisosAPI.CQRS.Handlers.CommandHandlers
{
    public class ModificarPermisoCommandHandler : IRequestHandler<ModificarPermisoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogEvent _logEvent;
        public ModificarPermisoCommandHandler(IUnitOfWork unitOfWork, ILogEvent logEvent)
        {
            _unitOfWork = unitOfWork;
            _logEvent = logEvent;
        }

        public async Task<bool> Handle(ModificarPermisoCommand request, CancellationToken cancellationToken)
        {
            var tipoPermiso = await _unitOfWork.TiposPermiso.GetByIdAsync(request.Permiso.TipoPermisoId);
            if (tipoPermiso == null)
            {
                throw new ArgumentException("Tipo de permiso no encontrado");
            }
            request.Permiso.TipoPermiso = tipoPermiso;

            _unitOfWork.Permisos.UpdateAsync(request.Permiso);

            try
            {
                await _unitOfWork.SaveAsync();
                await _logEvent.LogEventToElasticsearchAsync("ModificarPermiso", request.Permiso);
                await _logEvent.LogEventToKafkaAsync("modify", request.Permiso);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
