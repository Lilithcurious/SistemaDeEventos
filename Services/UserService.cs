using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDTO> CreateUser(string name, string email, string password)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = password
        };

        await _userRepository.AddUser(user);

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<List<UserResponseDTO>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return users.Select(user => new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        }).ToList(); 
    }

    public async Task<UserResponseDTO> GetUserById(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDTO> UpdateUser(Guid id, string name, string email, string password)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Name = name;
        user.Email = email;
        user.Password = password;

        await _userRepository.UpdateUser(user);

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

