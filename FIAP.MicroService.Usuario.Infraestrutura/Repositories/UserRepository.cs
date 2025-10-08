using FIAP.MicroService.Usuario.Dominio;
using FIAP.MicroService.Usuario.Dominio.Entidades;
using FIAP.MicroService.Usuario.Dominio.Interfaces;
using FIAP.MicroService.Usuario.Infraestrutura;
using FIAP.MicroService.Usuario.Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FIAP.MicroService.Jogos.Infraestrutura.Repositories
{
    public class UserRepository : Usuario.Dominio.Entidades.IUserRepository
    {
        private readonly UserDbContext? _users;

        public async Task<Usuario.Dominio.Entidades.IUserRepository?> GetByIdAsync(Guid id)
        {
            return await _users.Users.FindAsync(id);
        }
        
    }
}