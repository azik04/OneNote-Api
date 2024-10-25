using Domain.DTO_s.Notes;
using FluentValidation;

namespace BLL.Validation.Notes;

public class NotesDTOValidation : AbstractValidator<NoteDTO>
{
    public NotesDTOValidation()
    {
        RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Mesaj tələb olunur.")
        .Length(1, 500).WithMessage("Mesaj 1-dən 500 simvola qədər olmalıdır.");
    }
}
