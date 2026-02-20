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

    public async Task<UserResponseDTO> CreateUser(string name, string email, string password, string? phone, DateOnly? birthDate, bool? isActive)
    {
        ValidateUserFields(name, email, password, birthDate);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Phone = phone,
            BirthDate = birthDate,
            IsActive = isActive ?? true
        };

        await _userRepository.AddUser(user);

        return MapToResponse(user);
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

        return users.Select(MapToResponse).ToList();
    }

    public async Task<UserResponseDTO> GetUserById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return MapToResponse(user);
    }

    public async Task<UserResponseDTO> UpdateUser(Guid id, string name, string email, string password, string? phone, DateOnly? birthDate, bool? isActive)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid user ID");

        var user = await _userRepository.GetUserById(id);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        ValidateUserFields(name, email, password, birthDate);

        user.Name = name;
        user.Email = email;
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        user.Phone = phone;
        user.BirthDate = birthDate;
        user.IsActive = isActive;

        await _userRepository.UpdateUser(user);

        return MapToResponse(user);
    }

    private static void ValidateUserFields(string name, string email, string password, DateOnly? birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (!email.Contains("@"))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long");

        if (birthDate.HasValue && birthDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Birth date cannot be in the future");
    }

    private static UserResponseDTO MapToResponse(User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            BirthDate = user.BirthDate,
            IsActive = user.IsActive
        };
    }
}
