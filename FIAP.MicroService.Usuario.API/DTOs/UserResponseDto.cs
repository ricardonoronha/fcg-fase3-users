namespace FIAP.MicroService.Usuario.API.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime DataCriacao { get; set; }
}