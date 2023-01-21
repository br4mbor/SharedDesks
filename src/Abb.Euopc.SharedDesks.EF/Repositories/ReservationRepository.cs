using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Enums;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal sealed class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    public ReservationRepository(SharedDesksContext context) : base(context)
    { }

    public override IEnumerable<Reservation> GetAll()
    {
        return Get()
            .Include(r => r.Desk)
            .ThenInclude(d => d.Area);
    }

    public override Task<List<Reservation>> GetAllAsync()
    {
        return GetAll()
            .AsQueryable()
            .ToListAsync();
    }

    public override Reservation? GetById(int id)
    {
        return Get(r => r.Id == id)
            .Include(r => r.Desk)
            .ThenInclude(d => d.Area)
            .SingleOrDefault();
    }

    public override Task<Reservation?> GetByIdAsync(int id)
    {
        return Get(r => r.Id == id)
            .Include(r => r.Desk)
            .ThenInclude(d => d.Area)
            .SingleOrDefaultAsync();
    }

    public IEnumerable<Reservation> GetUpcomingReservationsByDeskId(int deskId)
    {
        return Get(r => r.DeskId == deskId && r.Date >= DateTime.Today);
    }

    public async Task<(List<Reservation>, int)> GetUsersUpcomingReservationsAsync(string userEmail, ReservationsFilterParameter parameter, int page, int pageSize)
    {
        var query = Get(r => r.Date.Date >= DateTime.Today);

        switch (parameter)
        {
            case ReservationsFilterParameter.CreatedBy:
                query = query.Where(r => r.CreatedByEmail == userEmail);
                break;
            case ReservationsFilterParameter.ReservedFor:
                query = query.Where(r => r.ReservedForEmail == userEmail);
                break;
            case ReservationsFilterParameter.Both:
            default:
                query = query.Where(r => r.CreatedByEmail == userEmail || r.ReservedForEmail == userEmail);
                break;
        }

        var count = await query.CountAsync();

        return (await query.Include(r => r.Desk)
            .ThenInclude(d => d.Area)
            .OrderBy(r => r.Date)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(), count);
    }

    public Task<bool> UserHasAnyReservationAsync(string userEmail, IEnumerable<DateTime> dates)
    {
        return Get().AnyAsync(r => r.ReservedForEmail == userEmail && dates.Contains(r.Date));
    }
}
