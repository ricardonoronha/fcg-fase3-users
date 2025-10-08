using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface IUserRepository
{
    // A interface deve definir que o método retorna uma Task<User?>
    Task<User?> GetByIdAsync(Guid id);

    // O mesmo para este método
    Task<User?> FindByUsernameAndPasswordAsync(string username, string password);
}
