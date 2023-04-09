using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using N5PermisosAPI.Controllers;
using N5PermisosAPI.CQRS.Commands;
using N5PermisosAPI.CQRS.Queries;
using N5PermisosAPI.DataAccess.Interfaces;
using N5PermisosAPI.Models;
using Xunit;

namespace N5PermisosAPI.Tests
{
    public class PermisosControllerTests
    {
        private PermisosController CreateController(Mock<IMediator> mediatorMock, Mock<ILogEvent> logEventMock)
        {
            return new PermisosController(mediatorMock.Object, logEventMock.Object);
        }

        [Fact]
        public async Task GetPermisos_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var logEventMock = new Mock<ILogEvent>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetPermisosQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(new List<Permiso>
             {
                new Permiso
                {
                    Id = 1,
                    NombreEmpleado = "John",
                    ApellidoEmpleado = "Doe",
                    FechaPermiso = DateTime.UtcNow,
                    TipoPermiso = new TipoPermiso
                    {
                        Id = 1,
                        Descripcion = "Vacation"
                    }
                }
             });

            var controller = CreateController(mediatorMock, logEventMock);

            // Act
            ActionResult<IEnumerable<Permiso>> result = await controller.GetPermisos();

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            var permisos = Assert.IsAssignableFrom<IEnumerable<Permiso>>(okResult.Value);


        }

        [Fact]
        public async Task SolicitarPermiso_ReturnsCreatedResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var logEventMock = new Mock<ILogEvent>();
            var permiso = new Permiso
            {
                NombreEmpleado = "Test",
                ApellidoEmpleado = "User",
                FechaPermiso = DateTime.Now,
                TipoPermiso = new TipoPermiso { Id = 1, Descripcion = "Test Description" }
            };

            mediatorMock.Setup(mediator => mediator.Send(It.IsAny<SolicitarPermisoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(1);
            var controller = CreateController(mediatorMock, logEventMock);
            // Act
            var result = await controller.SolicitarPermiso(permiso);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var id = Assert.IsType<int>(((OkObjectResult)result.Result).Value);
            Assert.Equal(1, id);
        }

        [Fact]
        public async Task ModificarPermiso_ReturnsOkResult()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var logEventMock = new Mock<ILogEvent>();
            int id = 1;
            var permiso = new Permiso
            {
                Id = 1,
                NombreEmpleado = "Test",
                ApellidoEmpleado = "User",
                FechaPermiso = DateTime.Now,
                TipoPermiso = new TipoPermiso { Id = 1, Descripcion = "Test Description" }
            };

            mediatorMock.Setup(mediator => mediator.Send(It.IsAny<ModificarPermisoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);
            var controller = CreateController(mediatorMock, logEventMock);
            // Act
            var result = await controller.ModificarPermiso(id, permiso);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}
