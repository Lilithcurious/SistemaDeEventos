using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Tests.Controllers;

[TestFixture]
public class UserControllerTests
{
    private static UserCreateRequestDTO CreateCreateRequest()
        => new()
        {
            Name = "Maria",
            Email = "maria@email.com",
            Password = "123456",
            Phone = "(11) 98765-4321",
            BirthDate = new DateOnly(1995, 3, 15),
            IsActive = true
        };

    private static UserUpdateRequestDTO CreateUpdateRequest()
        => new()
        {
            Name = "Maria Update",
            Email = "maria2@email.com",
            Password = "abcdef",
            Phone = "(11) 99999-0000",
            BirthDate = new DateOnly(1990, 7, 22),
            IsActive = true
        };

    private static UserResponseDTO CreateResponse(Guid? id = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "(11) 98765-4321",
            BirthDate = new DateOnly(1995, 3, 15),
            IsActive = true
        };

    [Test]
    public async Task CreateUser_ModelStateInvalido_DeveRetornarBadRequest()
    {
        var service = new Mock<IUserService>();
        var controller = new UserController(service.Object);

        controller.ModelState.AddModelError("Name", "Obrigatório");

        var result = await controller.CreateUser(CreateCreateRequest());

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        service.Verify(s => s.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()), Times.Never);
    }

    [Test]
    public async Task CreateUser_DeveRetornarCreatedAtAction()
    {
        var created = CreateResponse(Guid.NewGuid());
        var service = new Mock<IUserService>();

        service.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()))
               .ReturnsAsync(created);

        var controller = new UserController(service.Object);

        var result = await controller.CreateUser(CreateCreateRequest());

        var createdAt = result.Result as CreatedAtActionResult;
        Assert.That(createdAt, Is.Not.Null);
        Assert.That(createdAt!.ActionName, Is.EqualTo(nameof(UserController.GetUserById)));

        var dto = createdAt.Value as UserResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(created.Id));

        service.Verify(s => s.CreateUser("Maria", "maria@email.com", "123456", "(11) 98765-4321", new DateOnly(1995, 3, 15), true), Times.Once);
    }

    [Test]
    public async Task CreateUser_ServiceLancaArgumentException_DeveRetornarBadRequest()
    {
        var service = new Mock<IUserService>();

        service.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()))
               .ThrowsAsync(new ArgumentException("Email inválido"));

        var controller = new UserController(service.Object);

        var result = await controller.CreateUser(CreateCreateRequest());

        var bad = result.Result as BadRequestObjectResult;
        Assert.That(bad, Is.Not.Null);
        Assert.That(bad!.Value, Is.EqualTo("Email inválido"));
    }

    [Test]
    public async Task GetAllUsers_QuandoServiceRetornaLista_DeveOk()
    {
        var service = new Mock<IUserService>();
        service.Setup(s => s.GetAllUsers())
               .ReturnsAsync(new List<UserResponseDTO> { CreateResponse(), CreateResponse() });

        var controller = new UserController(service.Object);

        var result = await controller.GetAllUsers();

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var list = ok!.Value as List<UserResponseDTO>;
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllUsers_QuandoServiceRetornaNull_DeveOkComListaVazia()
    {
        var service = new Mock<IUserService>();
        service.Setup(s => s.GetAllUsers())
               .ReturnsAsync((List<UserResponseDTO>?)null);

        var controller = new UserController(service.Object);

        var result = await controller.GetAllUsers();

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var list = ok!.Value as List<UserResponseDTO>;
        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetUserById_DeveRetornarOk()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.GetUserById(id))
               .ReturnsAsync(CreateResponse(id));

        var controller = new UserController(service.Object);

        var result = await controller.GetUserById(id);

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as UserResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetUserById_ArgumentException_DeveBadRequest()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.GetUserById(id))
               .ThrowsAsync(new ArgumentException("Id inválido"));

        var controller = new UserController(service.Object);

        var result = await controller.GetUserById(id);

        var bad = result.Result as BadRequestObjectResult;
        Assert.That(bad, Is.Not.Null);
        Assert.That(bad!.Value, Is.EqualTo("Id inválido"));
    }

    [Test]
    public async Task GetUserById_KeyNotFound_DeveNotFound()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.GetUserById(id))
               .ThrowsAsync(new KeyNotFoundException());

        var controller = new UserController(service.Object);

        var result = await controller.GetUserById(id);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task UpdateUser_ModelStateInvalido_DeveBadRequest()
    {
        var service = new Mock<IUserService>();
        var controller = new UserController(service.Object);

        controller.ModelState.AddModelError("Email", "Obrigatório");

        var result = await controller.UpdateUser(Guid.NewGuid(), CreateUpdateRequest());

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        service.Verify(s => s.UpdateUser(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()), Times.Never);
    }

    [Test]
    public async Task UpdateUser_DeveOk()
    {
        var id = Guid.NewGuid();
        var updated = CreateResponse(id);

        var service = new Mock<IUserService>();
        service.Setup(s => s.UpdateUser(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()))
               .ReturnsAsync(updated);

        var controller = new UserController(service.Object);

        var result = await controller.UpdateUser(id, CreateUpdateRequest());

        var ok = result.Result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);

        var dto = ok!.Value as UserResponseDTO;
        Assert.That(dto, Is.Not.Null);
        Assert.That(dto!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task UpdateUser_ArgumentException_DeveBadRequest()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.UpdateUser(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()))
               .ThrowsAsync(new ArgumentException("Email inválido"));

        var controller = new UserController(service.Object);

        var result = await controller.UpdateUser(id, CreateUpdateRequest());

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateUser_KeyNotFound_DeveNotFound()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.UpdateUser(id, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<DateOnly?>(), It.IsAny<bool?>()))
               .ThrowsAsync(new KeyNotFoundException());

        var controller = new UserController(service.Object);

        var result = await controller.UpdateUser(id, CreateUpdateRequest());

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteUser_QuandoTrue_DeveNoContent()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.DeleteUser(id))
               .ReturnsAsync(true);

        var controller = new UserController(service.Object);

        var result = await controller.DeleteUser(id);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task DeleteUser_QuandoFalse_DeveNotFound()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.DeleteUser(id))
               .ReturnsAsync(false);

        var controller = new UserController(service.Object);

        var result = await controller.DeleteUser(id);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task DeleteUser_KeyNotFound_DeveNotFound()
    {
        var id = Guid.NewGuid();
        var service = new Mock<IUserService>();

        service.Setup(s => s.DeleteUser(id))
               .ThrowsAsync(new KeyNotFoundException());

        var controller = new UserController(service.Object);

        var result = await controller.DeleteUser(id);

        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }
}
