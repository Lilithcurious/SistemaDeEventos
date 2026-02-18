using Microsoft.AspNetCore.Mvc;
using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDTO>> CreateUser([FromBody] UserCreateRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.CreateUser(request.Name, request.Email, request.Password);

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users ?? new List<UserResponseDTO>());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(Guid id, [FromBody] UserUpdateRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUser = await _userService.UpdateUser(id, request.Name, request.Email, request.Password);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return result ? NoContent() : NotFound();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}



