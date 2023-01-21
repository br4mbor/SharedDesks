using Abb.Euopc.SharedDesks.Domain.Entities;
using FluentValidation;

namespace Abb.Euopc.SharedDesks.Domain.Validators;

public sealed class DeskItemValidator : AbstractValidator<DeskItem>
{
    public DeskItemValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(d => d.Type)
            .NotNull();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DeskItem>.CreateWithOptions((DeskItem)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
