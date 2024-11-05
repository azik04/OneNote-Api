using Domain.DTO_s.Users;
using FluentValidation;

public class LogInDTOValidator : AbstractValidator<LogInDTO>
{
    public LogInDTOValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("İstifadəçi adı tələb olunur.")
            .Length(1, 100).WithMessage("İstifadəçi adı 1-dən 100 simvola qədər olmalıdır.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə tələb olunur.")
            .Length(8, 100).WithMessage("Şifrə minimum 8 simvol uzunluğunda olmalıdır.");
    }
}