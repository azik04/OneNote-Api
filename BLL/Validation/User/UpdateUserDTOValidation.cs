using Domain.DTO_s.Users;
using FluentValidation;

namespace BLL.Validation.User;

public class UpdateUserDTOValidation : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidation()
    {
        RuleFor(x => x.UserName)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 20).WithMessage("Mesaj 1-dən 20 simvola qədər olmalıdır.");
    }
}
