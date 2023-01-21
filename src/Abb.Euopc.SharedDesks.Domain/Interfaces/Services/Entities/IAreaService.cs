using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IAreaService : IEntityService<Area>
{
    Task<bool> AddWithImageAsync(Area model, byte[]? image);
    Task<bool> UpdateWithImageAsync(Area model, byte[]? image);
}
