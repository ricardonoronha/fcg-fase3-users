using FIAP.MicroService.Usuario.Dominio.Entidades;
using FluentValidation;

namespace FIAP.MicroService.Usuario.Dominio.Validacoes;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("O nome de usuário não pode ser vazio!")
            .MinimumLength(3).WithMessage("O nome de usuário deve ter no mínimo 3 caracteres.");
        
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório!")
            .EmailAddress().WithMessage("O formato do e-mail é inválido.");
    }
}