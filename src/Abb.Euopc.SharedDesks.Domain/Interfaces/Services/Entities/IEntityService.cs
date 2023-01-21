using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

public interface IEntityService<TEntity>
    where TEntity : Entity, new()
{
    ICollection<TEntity> GetAll();
    Task<ICollection<TEntity>> GetAllAsync();
    TEntity? GetById(int id);
    Task<TEntity?> GetByIdAsync(int id);
    bool Add(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
    bool DeleteById(int id);
}
