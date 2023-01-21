using Abb.Euopc.SharedDesks.Domain.Entities;
using FluentValidation;

namespace Abb.Euopc.SharedDesks.Domain.Validators;

public sealed class AreaValidator : AbstractValidator<Area>
{
    public AreaValidator()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Floor)
            .InclusiveBetween(1, 10);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Area>.CreateWithOptions((Area)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
