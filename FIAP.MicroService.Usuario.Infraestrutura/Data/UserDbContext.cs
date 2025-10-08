using FIAP.MicroService.Usuario.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace FIAP.MicroService.Usuario.Infraestrutura.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    // Mantendo o código para criar o usuário 'admin' que você já tinha
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("9f4eb7ce-9c51-42b2-84bd-701cf961ddca"),
                Username = "admin",
                Password = "adminpassword",
                Email = "admin@email.com",
                Role = "admin"
            }
        );
    }
}