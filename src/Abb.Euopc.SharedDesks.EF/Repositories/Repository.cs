using System.Linq.Expressions;
using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Abb.Euopc.SharedDesks.EF.Repositories;

internal class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly SharedDesksContext _context;

    protected DbSet<TEntity> Set => _context.Set<TEntity>();

    public Repository(SharedDesksContext context)
    {
        _context = context;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return Get();
    }

    public virtual Task<List<TEntity>> GetAllAsync()
    {
        return Get().ToListAsync();
    }

    public virtual async Task<(List<TEntity>, int)> GetAllPagedAsync(int page, int pageSize)
    {
        var query = Get();

        return (await query.Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(), await query.CountAsync());
    }

    public virtual TEntity? GetById(int id)
    {
        return Set.Find(id);
    }

    public virtual Task<TEntity?> GetByIdAsync(int id)
    {
        return Set.FindAsync(id).AsTask();
    }

    public virtual TEntity Add(TEntity entity)
    {
        var addedEntity = Set.Add(entity).Entity;

        return addedEntity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Set.AddRange(entities);
    }

    public virtual TEntity Update(TEntity entity)
    {
        var updatedEntity = Set.Update(entity).Entity;

        return updatedEntity;
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        Set.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public void DeleteById(int id)
    {
        var entityToDelete = GetById(id);

        if (entityToDelete is null)
        {
            return;
        }

        Set.Remove(entityToDelete);
    }

    protected IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? where = null)
    {
        if (where is not null)
        {
            return Set.Where(where).AsQueryable();
        }

        return Set.AsQueryable();
    }
}
