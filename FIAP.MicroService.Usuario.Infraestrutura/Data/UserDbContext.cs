using FIAP.MicroService.Usuario.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FIAP.MicroService.Usuario.Infraestrutura.Data
{
    // A classe deve herdar de DbContext
    public class UserDbContext : DbContext
    {
        // Construtor obrigatório para Injeção de Dependência
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        // Mapeia a classe Jogo para a tabela "Users" no banc de dados.
        public DbSet<IUserRepository> Users { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IUserRepository>().HasData(
                new IUserRepository
                {
                    Id = Guid.Parse("9f4ab7ce-9c51-42b2-86bd-701c9f61ddca"),
                    Username = "admin",
                    Password = "adminpassword",
                    Email = "admin@email.com",
                    Role = "admin",
                    DataCriacao = DateTime.UtcNow
                }
            );
            modelBuilder.Entity<IUserRepository>().HasData(
                new IUserRepository
                {
                    Id = Guid.Parse("11111111-9c51-42b2-86bd-701c9f61ddca"),
                    Username = "user1",
                    Password = "password1",
                    Email = "user1@email.com",
                    Role = "admin",
                    DataCriacao = DateTime.UtcNow
                }
            );
        }

    }
}
