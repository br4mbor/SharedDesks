using Abb.Euopc.SharedDesks.Domain.Entities;
using FluentValidation;

namespace Abb.Euopc.SharedDesks.Domain.Validators;

public sealed class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(r => r.CreatedByEmail)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(r => r.ReservedForEmail)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(r => r.Date)
            .GreaterThanOrEqualTo(DateTime.Today);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Reservation>.CreateWithOptions((Reservation)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
