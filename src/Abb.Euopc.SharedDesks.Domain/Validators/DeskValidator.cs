using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using FluentValidation;

namespace Abb.Euopc.SharedDesks.Domain.Validators;

public sealed class DeskValidator : AbstractValidator<Desk>
{
    private readonly IDeskService _deskService;

    public DeskValidator(IDeskService deskService)
    {
        _deskService = deskService;

        RuleFor(d => d.Label)
            .NotEmpty()
            .MaximumLength(10)
            .MustAsync(async (desk, label, cancellationToken) => await IsUniqueLabelAsync(desk.Id, label))
            .WithMessage("Desk label is not unique");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Desk>.CreateWithOptions((Desk)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };

    private Task<bool> IsUniqueLabelAsync(int deskId, string label)
    {
        return _deskService.IsUniqueLabelAsync(deskId, label);
    }
}
