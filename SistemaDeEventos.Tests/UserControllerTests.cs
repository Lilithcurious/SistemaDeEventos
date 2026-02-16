using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SistemaDeEventos.Controllers;
using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaDeEventos.Tests.Controllers
{
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task CreateUser_ValidRequest_ReturnsCreatedAtAction()
        {
            var request = new UserCreateRequestDTO
            {
                Name = "João Silva",
                Email = "joao@email.com",
                Password = "123456"
            };

            var userResponse = new UserResponseDTO
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email
            };

            _mockUserService
                .Setup(s => s.CreateUser(request.Name, request.Email, request.Password))
                .ReturnsAsync(userResponse);

            var result = await _controller.CreateUser(request);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

            var createdResult = (CreatedAtActionResult)result.Result!;
            Assert.That(createdResult.Value, Is.InstanceOf<UserResponseDTO>());

            var user = (UserResponseDTO)createdResult.Value!;
            Assert.That(user.Id, Is.EqualTo(userResponse.Id));
            Assert.That(user.Name, Is.EqualTo(request.Name));
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkWithUsersList()
        {
            var users = new List<UserResponseDTO>
            {
                new UserResponseDTO { Id = Guid.NewGuid(), Name = "João", Email = "joao@email.com" },
                new UserResponseDTO { Id = Guid.NewGuid(), Name = "Maria", Email = "maria@email.com" }
            };

            _mockUserService
                .Setup(s => s.GetAllUsers())
                .ReturnsAsync(users);

            var result = await _controller.GetAllUsers();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            Assert.That(okResult.Value, Is.InstanceOf<List<UserResponseDTO>>());

            var usersResult = (List<UserResponseDTO>)okResult.Value!;
            Assert.That(usersResult.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserById_ExistingUser_ReturnsOk()
        {
            var userId = Guid.NewGuid();

            var user = new UserResponseDTO
            {
                Id = userId,
                Name = "João Silva",
                Email = "joao@email.com"
            };

            _mockUserService
                .Setup(s => s.GetUserById(userId))
                .ReturnsAsync(user);

            var result = await _controller.GetUserById(userId);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            var returnedUser = (UserResponseDTO)okResult.Value!;

            Assert.That(returnedUser.Id, Is.EqualTo(userId));
        }

        [Test]
        public void GetUserById_NonExistingUser_ThrowsException()
        {
            var userId = Guid.NewGuid();

            _mockUserService
                .Setup(s => s.GetUserById(userId))
                .ThrowsAsync(new Exception("User not found"));

            Assert.ThrowsAsync<Exception>(async () =>
                await _controller.GetUserById(userId));
        }

        [Test]
        public async Task UpdateUser_ValidRequest_ReturnsOk()
        {
            var userId = Guid.NewGuid();

            var request = new UserUpdateRequestDTO
            {
                Name = "João Atualizado",
                Email = "joao@novo.com",
                Password = "newpass123"
            };

            var updatedUser = new UserResponseDTO
            {
                Id = userId,
                Name = request.Name,
                Email = request.Email
            };

            _mockUserService
                .Setup(s => s.UpdateUser(userId, request.Name, request.Email, request.Password))
                .ReturnsAsync(updatedUser);

            var result = await _controller.UpdateUser(userId, request);

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;
            var userResult = (UserResponseDTO)okResult.Value!;

            Assert.That(userResult.Name, Is.EqualTo(request.Name));
        }

        [Test]
        public void UpdateUser_NonExistingUser_ThrowsException()
        {
            var userId = Guid.NewGuid();

            var request = new UserUpdateRequestDTO
            {
                Name = "Novo Nome",
                Email = "novo@email.com",
                Password = "123"
            };

            _mockUserService
                .Setup(s => s.UpdateUser(userId, request.Name, request.Email, request.Password))
                .ThrowsAsync(new Exception("User not found"));

            Assert.ThrowsAsync<Exception>(async () =>
                await _controller.UpdateUser(userId, request));
        }
    }
}
