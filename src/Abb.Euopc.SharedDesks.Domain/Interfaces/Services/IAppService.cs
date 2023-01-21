namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services;

public interface IAppService
{
    IEnumerable<DateTime> PossibleDates { get; }
}

