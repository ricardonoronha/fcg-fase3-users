using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;

namespace FIAP.MicroService.Usuario.Infraestrutura.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;

    public UserRepository(UserDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<User?> FindByUsernameAndPasswordAsync(string username, string password)
    {
       return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(t => string.Equals(username,t.Username) && string.Equals(password, t.Password));
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }
}