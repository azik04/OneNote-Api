using Domain.DTO_s.Sections;
using FluentValidation;

namespace BLL.Validation.Sections;

public class SectionDTOValidation : AbstractValidator<SectionDTO>
{
    public SectionDTOValidation()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Mesaj tələb olunur.")
           .Length(1, 50).WithMessage("Mesaj 1-dən 50 simvola qədər olmalıdır.");
    }
}
