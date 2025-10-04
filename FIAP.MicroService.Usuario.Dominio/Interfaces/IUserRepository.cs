using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> FindByUsernameAndPasswordAsync(string username, string password);
}