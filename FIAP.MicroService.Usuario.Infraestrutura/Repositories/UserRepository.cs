using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

// Garanta que o namespace esteja exatamente assim
namespace FIAP.MicroService.Usuario.Infraestrutura.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> FindByUsernameAndPasswordAsync(string username, string password)
    {
        // Em um projeto real, a senha seria comparada com um hash
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }
}