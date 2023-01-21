using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IDeskItemService : IEntityService<DeskItem>
{
    Task<ICollection<DeskItem>> GetAllActiveAsync();
}
