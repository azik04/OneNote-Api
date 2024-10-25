using Domain.DTO_s.Users;
using FluentValidation;

namespace BLL.Validation.User;

public class CreateUserDTOValidation : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidation()
    {
        RuleFor(x => x.UserName)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 20).WithMessage("Mesaj 1-dən 20 simvola qədər olmalıdır.");
        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(8, 20).WithMessage("Mesaj 8-dən 20 simvola qədər olmalıdır.");
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .EmailAddress().WithMessage("Email formatı yanlışdır.")
            .Length(1, 50).WithMessage("Email 1-dən 50 simvola qədər olmalıdır.");
    }
}
