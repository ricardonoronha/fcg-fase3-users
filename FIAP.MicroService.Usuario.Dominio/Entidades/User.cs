namespace FIAP.MicroService.Usuario.Dominio.Entidades;

// Se a sua classe User implementa uma interface, ficaria assim: public class User : IEntidadeBase
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}