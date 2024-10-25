using Domain.DTO_s.Folders;
using FluentValidation;

namespace BLL.Validation.Folders;

public class FolderDTOValidation : AbstractValidator<FolderDTO>
{
    public FolderDTOValidation()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 50).WithMessage("Mesaj 1-dən 50 simvola qədər olmalıdır.");
    }
}
