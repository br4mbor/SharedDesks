using Abb.Euopc.SharedDesks.Application.Options;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Abb.Euopc.SharedDesks.Application.Services;

internal sealed class AppService : IAppService
{
	private readonly AppOptions _appOptions;

    private IEnumerable<DateTime>? _possibleDates = null;

    public IEnumerable<DateTime> PossibleDates
    {
        get
        {
            if (_possibleDates is null || _possibleDates.First() != DateTime.Today)
            {
                PossibleDates = GenerateDates(DateTime.Today).ToList();
            }

            return _possibleDates!;
        }
        private set
        {
            _possibleDates = value;
        }
    }

    public AppService(IOptions<AppOptions> appOptions)
	{
		_appOptions = appOptions.Value;
	}

    private IEnumerable<DateTime> GenerateDates(DateTime start)
    {
        var i = 0;
        var skipped = 0;

        while (i < _appOptions.DeskFilterDatesCount)
        {
            var day = start.AddDays(i + skipped);

            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                skipped++;
                continue;
            }

            yield return day;

            i++;
        }
    }
}

