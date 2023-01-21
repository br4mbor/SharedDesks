using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

public interface IRepository
{ }

public interface IRepository<TEntity> : IRepository where TEntity : Entity, new()
{
    IEnumerable<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
    Task<(List<TEntity>, int)> GetAllPagedAsync(int page, int pageSize);
    TEntity? GetById(int id);
    Task<TEntity?> GetByIdAsync(int id);
    TEntity Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    TEntity Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteById(int id);
}
