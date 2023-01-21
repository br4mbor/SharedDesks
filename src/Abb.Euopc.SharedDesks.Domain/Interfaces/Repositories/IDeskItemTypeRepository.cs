using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

public interface IDeskItemTypeRepository : IRepository<DeskItemType>
{
    IEnumerable<DeskItemType> GetAllActive();
    Task<List<DeskItemType>> GetAllActiveAsync();
}

