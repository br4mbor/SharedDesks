using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Context;

public interface IDomainContext : IDisposable, IAsyncDisposable
{
    IAreaRepository AreaRepository { get; }
    IDeskItemRepository DeskItemRepository { get; }
    IDeskItemTypeRepository DeskItemTypeRepository { get; }
    IDeskRepository DeskRepository { get; }
    IReservationRepository ReservationRepository { get; }
    IRepository<DeskItemToDesk> DeskItemToDeskRepository { get; }

    void Save();
    Task SaveAsync();
}
