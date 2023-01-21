using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal sealed class DeskItemTypeRepository : Repository<DeskItemType>, IDeskItemTypeRepository
{
    public DeskItemTypeRepository(SharedDesksContext context) : base(context)
    { }

    public IEnumerable<DeskItemType> GetAllActive()
    {
        return Get(dit => !dit.IsDeleted)
            .OrderBy(dit => dit.Name);
    }

    public Task<List<DeskItemType>> GetAllActiveAsync()
    {
        return GetAllActive()
            .AsQueryable()
            .ToListAsync();
    }
}
