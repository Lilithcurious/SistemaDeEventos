using SistemaDeEventos.Models;

namespace SistemaDeEventos.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(Guid id);
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(User user);
}
