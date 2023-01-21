using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Enums;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    IEnumerable<Reservation> GetUpcomingReservationsByDeskId(int deskId);
    Task<(List<Reservation> Items, int TotalItems)> GetUsersUpcomingReservationsAsync(string userEmail, ReservationsFilterParameter parameter, int page, int pageSize);
    Task<bool> UserHasAnyReservationAsync(string userEmail, IEnumerable<DateTime> dates);
}

