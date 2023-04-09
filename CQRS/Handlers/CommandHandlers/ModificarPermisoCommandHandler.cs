using MediatR;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.DataAccess.Interfaces;

namespace N5PermisosAPI.CQRS.Handlers.CommandHandlers
{
    public class ModificarPermisoCommandHandler : IRequestHandler<ModificarPermisoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ModificarPermisoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
