using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;


namespace FIAP.MicroService.Usuario.Infraestrutura.Repositories;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public MockUserRepository()
    {
        // Adicionando usuários de exemplo em memória.
        // Usando o mesmo Guid do seu DbContext para o admin para consistência.
        _users.Add(new User { Id = Guid.Parse("9f4eb7ce-9c51-42b2-84bd-701cf961ddca"), Username = "admin", Password = "adminpassword", Email = "admin@email.com", Role = "admin" });
        _users.Add(new User { Id = Guid.NewGuid(), Username = "user1", Password = "password1", Email = "user1@email.com", Role = "user" });
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