using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Enums;
using Abb.Euopc.SharedDesks.Domain.Responses.Reservation;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IReservationService : IEntityService<Reservation>
{
    Task<CreateReservationResponse> CreateReservationsAsync(IEnumerable<DateTime>? selectedDates, int deskId, string reservedForEmail, string createdByEmail);
    Task<(ICollection<Reservation> Items, int TotalItems)> GetUsersUpcomingReservationsAsync(string currentUserEmail, ReservationsFilterParameter parameter, int page, int pageSize);
    Task<bool> Cancel(Reservation reservation, string cancellingUserEmail);
}
