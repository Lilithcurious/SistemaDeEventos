using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Services;
using SistemaDeEventos.Models;

namespace SistemaDeEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] UserCreateRequestDTO request)
        {
            var user = await _userService.CreateUser(request.Name, request.Email, request.Password);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(Guid id, [FromBody] UserUpdateRequestDTO request)
        {
            var updatedUser = await _userService.UpdateUser(id, request.Name, request.Email, request.Password);
            if (updatedUser == null)
                return NotFound();
            return Ok(updatedUser);
        }
    }
}

