namespace SistemaDeEventos.Interfaces;

using SistemaDeEventos.DTOs.User;

public interface IUserService
{
    Task<List<UserResponseDTO>> GetAllUsers();
    Task<UserResponseDTO> GetUserById(Guid id);
    Task<UserResponseDTO> UpdateUser(Guid id, string name, string email, string password, string? phone, DateOnly? birthDate, bool? isActive);
    Task<UserResponseDTO> CreateUser(string name, string email, string password, string? phone, DateOnly? birthDate, bool? isActive);
    Task<bool> DeleteUser(Guid id);
}
