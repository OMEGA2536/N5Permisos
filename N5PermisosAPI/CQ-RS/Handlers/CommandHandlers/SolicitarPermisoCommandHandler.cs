using MediatR;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.DataAccess.Interfaces;

namespace N5PermisosAPI.CQRS.Handlers.CommandHandlers
{
    public class SolicitarPermisoCommandHandler : IRequestHandler<SolicitarPermisoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SolicitarPermisoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            return request.Permiso.Id;
        }
    }
}
