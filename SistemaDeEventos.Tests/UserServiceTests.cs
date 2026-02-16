using NUnit.Framework;
using Moq;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.DTOs.User;

public class UserServiceTests
{
    private Mock<IUserRepository> _mockRepository;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }

    [Test]
    public async Task CreateUser_DeveCriarUsuario_ComDadosValidos()
    {
        string name = "Luciana";
        string email = "lu@email.com";
        string password = "123";

        var result = await _userService.CreateUser(name, email, password);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(name));
        Assert.That(result.Email, Is.EqualTo(email));

        _mockRepository.Verify(r =>
            r.AddUser(It.Is<User>(u =>
                u.Name == name &&
                u.Email == email &&
                u.Password == password
            )),
            Times.Once);
    }


    [Test]
    public async Task GetAllUsers_DeveRetornarListaMapeada()
    {
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = "A", Email = "a@email.com" },
            new User { Id = Guid.NewGuid(), Name = "B", Email = "b@email.com" }
        };

        _mockRepository
            .Setup(r => r.GetAllUsers())
            .ReturnsAsync(users);

        var result = await _userService.GetAllUsers();

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("A"));
        Assert.That(result[1].Email, Is.EqualTo("b@email.com"));
    }

    [Test]
    public async Task GetUserById_DeveRetornarUsuario_QuandoExiste()
    {

        var id = Guid.NewGuid();

        var user = new User
        {
            Id = id,
            Name = "Luciana",
            Email = "lu@email.com"
        };

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync(user);

        var result = await _userService.GetUserById(id);

        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.Name, Is.EqualTo("Luciana"));
    }

    [Test]
    public void GetUserById_DeveLancarExcecao_QuandoNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync((User?)null);

        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _userService.GetUserById(id));

        Assert.That(ex!.Message, Is.EqualTo("User not found"));
    }

    [Test]
    public async Task UpdateUser_DeveAtualizarDados_QuandoUsuarioExiste()
    {
        var id = Guid.NewGuid();

        var user = new User
        {
            Id = id,
            Name = "Old",
            Email = "old@email.com",
            Password = "oldpass"
        };

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync(user);

        string newName = "New";
        string newEmail = "new@email.com";
        string newPassword = "newpass";

        var result = await _userService.UpdateUser(id, newName, newEmail, newPassword);

        Assert.That(result.Name, Is.EqualTo(newName));
        Assert.That(result.Email, Is.EqualTo(newEmail));

        _mockRepository.Verify(r =>
            r.UpdateUser(It.Is<User>(u =>
                u.Name == newName &&
                u.Email == newEmail &&
                u.Password == newPassword
            )),
            Times.Once);
    }

    [Test]
    public void UpdateUser_DeveLancarExcecao_QuandoUsuarioNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync((User?)null);

        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _userService.UpdateUser(id, "X", "x@email.com", "123"));

        Assert.That(ex!.Message, Is.EqualTo("User not found"));
    }
}
