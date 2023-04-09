using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRS.Commands;
using MediatR;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.DataAccess.Interfaces;

namespace CQRS.Handlers.CommandHandlers
{
    internal class CrearTipoPermisoCommandHandler : IRequestHandler<CrearTipoPermisoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrearTipoPermisoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CrearTipoPermisoCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.TiposPermiso.CreateAsync(request.TipoPermiso);
            await _unitOfWork.SaveAsync();
            return request.TipoPermiso.Id;
        }
    }
}
