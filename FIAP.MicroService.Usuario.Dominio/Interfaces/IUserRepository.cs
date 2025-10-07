using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface IUserRepository
{
    Task<Entidades.IUserRepository?> GetByIdAsync(Guid id);
    Task<Entidades.IUserRepository?> FindByUsernameAndPasswordAsync(string username, string password);
}