using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
