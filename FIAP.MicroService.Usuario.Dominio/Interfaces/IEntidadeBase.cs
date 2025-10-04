namespace FIAP.MicroService.Usuario.Dominio.Interfaces;

public interface IEntidadeBase
{
    Guid Id { get; set; }
    DateTime DataCriacao { get; set; }
    string UsuarioCriador { get; set; }
    DateTime DataAtualizacao { get; set; }
    string UsuarioAtualizador { get; set; }
}

