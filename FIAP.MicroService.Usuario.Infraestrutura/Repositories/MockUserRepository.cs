using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;

namespace FIAP.MicroService.Usuario.Infraestrutura.Repositories;

public class UserDbContext : Dominio.Interfaces.IUserRepository
{
    private readonly List<Dominio.Entidades.IUserRepository> _users = new();

    public UserDbContext()
    {
        _users.Add(new Dominio.Entidades.IUserRepository { Id = Guid.NewGuid(), Username = "user1", Password = "password1", Email = "user1@email.com", Role = "user", DataCriacao = DateTime.UtcNow });
        _users.Add(new Dominio.Entidades.IUserRepository { Id = Guid.NewGuid(), Username = "admin", Password = "adminpassword", Email = "admin@email.com", Role = "admin", DataCriacao = DateTime.UtcNow });
    }

    public Task<Dominio.Entidades.IUserRepository?> GetByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<Dominio.Entidades.IUserRepository?> FindByUsernameAndPasswordAsync(string username, string password)
    {
        var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        return Task.FromResult(user);
    }
}