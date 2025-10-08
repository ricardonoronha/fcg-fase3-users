using FIAP.MicroService.Usuario.Dominio.Entidades;

namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface ITokenService
{
    // CORREÇÃO: O método deve receber um objeto do tipo 'User'.
    string GenerateToken(User user);
}