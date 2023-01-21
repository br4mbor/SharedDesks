using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Requests;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IDeskService : IEntityService<Desk>
{
    Task<(ICollection<Desk> Items, int TotalItems)> GetAllPagedAsync(int page, int pageSize, string? searchString = null);
    Task<ICollection<Desk>> GetByFilterAsync(DeskFilterRequest deskFilter);
    Task<ICollection<Desk>> GetAvailableDesksAsync(DateTime date, int areaId);
    Task<bool> AddDeskWithImageAsync(Desk model, byte[]? image);
    Task<bool> UpdateDeskWithImageAsync(Desk model, byte[]? image);
    Task<bool> IsUniqueLabelAsync(int deskId, string label);
}
