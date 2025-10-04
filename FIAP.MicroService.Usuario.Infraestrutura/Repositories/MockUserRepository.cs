using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;

namespace FIAP.MicroService.Usuario.Infraestrutura.Repositories;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public MockUserRepository()
    {
        _users.Add(new User { Id = Guid.NewGuid(), Username = "user1", Password = "password1", Email = "user1@email.com", Role = "user", DataCriacao = DateTime.UtcNow });
        _users.Add(new User { Id = Guid.NewGuid(), Username = "admin", Password = "adminpassword", Email = "admin@email.com", Role = "admin", DataCriacao = DateTime.UtcNow });
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User?> FindByUsernameAndPasswordAsync(string username, string password)
    {
        var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);
        return Task.FromResult(user);
    }
}