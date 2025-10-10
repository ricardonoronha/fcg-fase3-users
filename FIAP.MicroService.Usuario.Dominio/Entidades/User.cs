
namespace FIAP.MicroService.Usuario.Dominio.Entidades;

public class User
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public string UsuarioCriador { get; set; } = string.Empty;
    public DateTime DataAtualizacao { get; set; }
    public string UsuarioAtualizador { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}