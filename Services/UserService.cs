using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<UserResponseDTO> CreateUser(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long");

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

    public async Task<bool> DeleteUser(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        var user = await _userRepository.GetUserById(id);
        if (user == null)
            return false;

        await _userRepository.DeleteUser(user);
        return true;
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
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDTO> UpdateUser(Guid id, string name, string email, string password)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long");

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