using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal sealed class AreaRepository : Repository<Area>, IAreaRepository
{
    public AreaRepository(SharedDesksContext context) : base(context)
    { }

    public override IEnumerable<Area> GetAll()
    {
        return Get()
            .OrderBy(a => a.Name)
            .ThenBy(a => a.Floor);
    }

    public override Task<List<Area>> GetAllAsync()
    {
        return GetAll()
            .AsQueryable()
            .ToListAsync();
    }
}
