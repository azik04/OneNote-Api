using Domain.DTO_s.Users;
using FluentValidation;

namespace BLL.Validation.User;

public class ChangePasswordDTOValidation : AbstractValidator<ChangePasswordDTO>
{
    public ChangePasswordDTOValidation()
    {
        RuleFor(x => x.OldPassword)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 20).WithMessage("Mesaj 1-dən 20 simvola qədər olmalıdır.");
        RuleFor(x => x.NewPassword)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(8, 20).WithMessage("Mesaj 8-dən 20 simvola qədər olmalıdır.");
        RuleFor(x => x.ConfirmNewPassword)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(8, 20).WithMessage("Mesaj 8-dən 20 simvola qədər olmalıdır.");
    }
}
