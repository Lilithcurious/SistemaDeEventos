using NUnit.Framework;
using Moq;
using SistemaDeEventos.Interfaces;
using SistemaDeEventos.Models;
using SistemaDeEventos.DTOs.User;

[TestFixture]
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

    //para cenário de sucesso

    [Test]
    public async Task CreateUser_DeveCriarUsuario_ComDadosValidos()
    {
        string name = "Luciana";
        string email = "lu@email.com";
        string password = "123456";

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

    //para cenário de falhas

    [Test]
    public void CreateUser_DeveLancarArgumentException_QuandoNomeVazio()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.CreateUser("", "email@email.com", "123456"));
    }

    [Test]
    public void CreateUser_DeveLancarArgumentException_QuandoEmailInvalido()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.CreateUser("Luciana", "emailinvalido", "123456"));
    }

    [Test]
    public void CreateUser_DeveLancarArgumentException_QuandoSenhaCurta()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.CreateUser("Luciana", "email@email.com", "123"));
    }

    //para cenário de sucesso

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

    //para cenário de sucesso no user id 

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

    //para cenário de falhas no user id 

    [Test]
    public void GetUserById_DeveLancarArgumentException_QuandoIdVazio()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.GetUserById(Guid.Empty));
    }

    [Test]
    public void GetUserById_DeveLancarKeyNotFoundException_QuandoNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync((User?)null);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _userService.GetUserById(id));
    }

    //para cenário de sucesso

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
        string newPassword = "newpass123";

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

    //para cenário de falhas

    [Test]
    public void UpdateUser_DeveLancarArgumentException_QuandoIdVazio()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.UpdateUser(Guid.Empty, "Nome", "email@email.com", "123456"));
    }

    [Test]
    public void UpdateUser_DeveLancarKeyNotFoundException_QuandoUsuarioNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync((User?)null);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _userService.UpdateUser(id, "Nome", "email@email.com", "123456"));
    }

    //para cenário de sucesso

    [Test]
    public async Task DeleteUser_DeveRetornarTrue_QuandoUsuarioExiste()
    {
        var id = Guid.NewGuid();
        var user = new User { Id = id };

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync(user);

        var result = await _userService.DeleteUser(id);

        Assert.That(result, Is.True);

        _mockRepository.Verify(r => r.DeleteUser(user), Times.Once);
    }

    //para cenário de falhas

    [Test]
    public void DeleteUser_DeveLancarArgumentException_QuandoIdVazio()
    {
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.DeleteUser(Guid.Empty));
    }

    [Test]
    public async Task DeleteUser_DeveRetornarFalse_QuandoUsuarioNaoExiste()
    {
        var id = Guid.NewGuid();

        _mockRepository
            .Setup(r => r.GetUserById(id))
            .ReturnsAsync((User?)null);

        var result = await _userService.DeleteUser(id);

        Assert.That(result, Is.False);
    }
}
