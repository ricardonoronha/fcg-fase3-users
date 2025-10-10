using FIAP.MicroService.Usuario.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FIAP.MicroService.Usuario.Infraestrutura.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("9f4ab7ce-9c51-42b2-86bd-701c9f61ddca"),
                    Username = "admin",
                    Password = "adminpassword",
                    Email = "admin@email.com",
                    Role = "admin",
                    DataCriacao = DateTime.UtcNow
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
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
