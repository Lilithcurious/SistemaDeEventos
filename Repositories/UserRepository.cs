using Microsoft.EntityFrameworkCore;
using SistemaDeEventos.Models;
using SistemaDeEventos.Interfaces;

namespace SistemaDeEventos.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventosContext _context;

    public UserRepository(EventosContext context)
    {
        _context = context;
    }

    // Adiciona um usuário no banco
    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    // Atualiza um usuário existente
    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // Deleta um usuário
    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    // Retorna todos os usuários
    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // Retorna um usuário pelo ID
    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
}



