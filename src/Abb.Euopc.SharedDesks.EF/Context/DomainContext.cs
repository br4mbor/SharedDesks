using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Repositories;

namespace Abb.Euopc.SharedDesks.EF.Context;

internal sealed class DomainContext : IDomainContext
{
    private readonly SharedDesksContext _dbContext;
    private readonly Dictionary<Type, IRepository> _repositories = new();

    public IAreaRepository AreaRepository => GetRepository<AreaRepository, Area>();

    public IDeskItemRepository DeskItemRepository => GetRepository<DeskItemRepository, DeskItem>();

    public IDeskItemTypeRepository DeskItemTypeRepository => GetRepository<DeskItemTypeRepository, DeskItemType>();

    public IDeskRepository DeskRepository => GetRepository<DeskRepository, Desk>();

    public IReservationRepository ReservationRepository => GetRepository<ReservationRepository, Reservation>();

    public IRepository<DeskItemToDesk> DeskItemToDeskRepository => GetRepository<Repository<DeskItemToDesk>, DeskItemToDesk>();

    public DomainContext(SharedDesksContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _dbContext.DisposeAsync();
    }

    private TRepository GetRepository<TRepository, TEntity>()
        where TRepository : Repository<TEntity>
        where TEntity : Entity, new()
    {
        if (_repositories.TryGetValue(typeof(TRepository), out var loadedRepository) && loadedRepository is TRepository loadedRepositoryTyped)
        {
            return loadedRepositoryTyped;
        }

        if (Activator.CreateInstance(typeof(TRepository), _dbContext) is not TRepository createdRepository)
        {
            throw new Exception("Repository not found.");
        }

        _repositories.Add(typeof(TRepository), createdRepository);

        return createdRepository;
    }
}
