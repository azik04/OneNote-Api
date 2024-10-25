using Domain.DTO_s.Users;
using FluentValidation;

namespace BLL.Validation.User;

public class LogInDTOValidation : AbstractValidator<LogInDTO>
{
    public LogInDTOValidation()
    {
        RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 20).WithMessage("Mesaj 1-dən 20 simvola qədər olmalıdır.");
        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(8, 20).WithMessage("Mesaj 8-dən 20 simvola qədər olmalıdır.");
    }
}
