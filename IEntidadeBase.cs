namespace FIAP.MicroService.Usuario.Dominio.Interfaces
{
    public interface IEntidadeBase
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string UsuarioCriador { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizador { get; set; }
    }
}