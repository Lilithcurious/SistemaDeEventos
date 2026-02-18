namespace SistemaDeEventos.Interfaces;

using SistemaDeEventos.DTOs.User;
using SistemaDeEventos.Models;

public interface IUserService
{
    Task<List<UserResponseDTO>> GetAllUsers();
    Task<UserResponseDTO> GetUserById(Guid id);
    Task<UserResponseDTO> UpdateUser(Guid id, string name, string email, string password);
    Task<UserResponseDTO> CreateUser(string name, string email, string password);
    Task<bool> DeleteUser(Guid id);
}

