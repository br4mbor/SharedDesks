using Abb.Euopc.SharedDesks.Domain.Entities;
using FluentValidation;

namespace Abb.Euopc.SharedDesks.Domain.Validators;

public sealed class DeskItemTypeValidator : AbstractValidator<DeskItemType>
{
    public DeskItemTypeValidator()
    {
        RuleFor(dit => dit.Name)
            .NotEmpty()
            .MaximumLength(30);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<DeskItemType>.CreateWithOptions((DeskItemType)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
