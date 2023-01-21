using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal sealed class DeskItemRepository : Repository<DeskItem>, IDeskItemRepository
{
    public DeskItemRepository(SharedDesksContext context) : base(context)
    { }

    public override IEnumerable<DeskItem> GetAll()
    {
        return Get()
            .Include(di => di.Type)
            .OrderBy(di => di.Name);
    }

    public override Task<List<DeskItem>> GetAllAsync()
    {
        return GetAll()
            .AsQueryable()
            .ToListAsync();
    }

    public Task<List<DeskItem>> GetAllActiveAsync()
    {
        return Get(di => !di.IsDeleted)
            .Include(di => di.Type)
            .ToListAsync();
    }

    public override DeskItem? GetById(int id)
    {
        return Get(di => di.Id == id)
            .Include(di => di.Type)
            .SingleOrDefault();
    }

    public override Task<DeskItem?> GetByIdAsync(int id)
    {
        return Get(di => di.Id == id)
            .Include(di => di.Type)
            .SingleOrDefaultAsync();
    }

    public override DeskItem Add(DeskItem entity)
    {
        entity.TypeId = entity.Type.Id;
        entity.Type = null!;

        return base.Add(entity);
    }

    public override DeskItem Update(DeskItem entity)
    {
        entity.TypeId = entity.Type.Id;
        entity.Type = null!;

        return base.Update(entity);
    }
}
