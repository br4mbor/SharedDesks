using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Requests;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

public interface IDeskRepository : IRepository<Desk>
{
    Task<(List<Desk>, int)> GetAllPagedAsync(int page, int pageSize, string? searchString = null);
    Task<List<Desk>> GetByFilterAsync(DeskFilterRequest deskFilter);
    Task<List<Desk>> GetAvailableDesksAsync(DateTime date, int areaId);
    Task<bool> IsUniqueLabelAsync(int deskId, string label);
}

