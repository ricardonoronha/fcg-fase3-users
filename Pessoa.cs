namespace FIAP.MicroService.Usuario.Dominio.Entidades
{
    public class Pessoa : IEntidadeBase
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string HashSenha { get; set; } = string.Empty;
        public bool EhAdministrador { get; set; }
        public bool EhAtivo { get; set; }

        public Pessoa() { }
    }
}