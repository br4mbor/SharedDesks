using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IDeskItemTypeService : IEntityService<DeskItemType>
{
    ICollection<DeskItemType> GetAllActive();
    Task<ICollection<DeskItemType>> GetAllActiveAsync();
}
