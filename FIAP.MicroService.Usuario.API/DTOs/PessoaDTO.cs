using System.ComponentModel.DataAnnotations;

namespace FIAP.MicroService.Usuario.API.DTOs
{
    public class PessoaDTO
    {
        [Required(ErrorMessage = "O Id deve ser obrigatorio")]
        public Guid Id { get; set; }
    }
}
