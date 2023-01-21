using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal abstract class EntityServiceBase<TEntity> : IEntityService<TEntity>
    where TEntity : Entity, new()
{
    protected readonly IDomainContextFactory _domainContextFactory;
    protected readonly ILogger _logger;

    protected const string GetAllBeginLogMessage = "Getting all entities of type {Type} from DB.";
    protected const string GetAllErrorLogMessage = "Something went wrong when getting all records of type {Type} from DB.";

    protected const string GetByIdBeginLogMessage = "Getting entity of type {Type} with Id {Id} from DB.";
    protected const string GetByIdFoundLogMessage = "Entity found. Entity data: {Entity}";
    protected const string GetByIdNotFoundLogMessage = "Entity of type {Type} with Id {Id} not found in DB.";
    protected const string GetByIdErrorLogMessage = "Something went wrong when getting entity of type {Type} from DB by Id {Id}.";

    protected const string AddBeginLogMessage = "Adding entity of type {Type} to DB. Entity data: {Entity}";
    protected const string AddSuccessLogMessage = "Entity succesfully added: {AddedEntity}";
    protected const string AddErrorLogMessage = "Something went wrong when adding entity of type {Type} to DB. \nEntity data: {Entity}";

    protected const string UpdateBeginLogMessage = "Updating entity of type {Type} in DB. Entity data: {Entity}";
    protected const string UpdateSuccessLogMessage = "Entity succesfully updated: {UpdatedEntity}";
    protected const string UpdateErrorLogMessage = "Something went wrong when updating entity of type {Type} in DB. \nEntity data: {Entity}";

    protected const string DeleteBeginLogMessage = "Deleting entity of type {Type} from DB. Entity data: {Entity}";
    protected const string DeleteSuccessLogMessage = "Entity succesfully deleted";
    protected const string DeleteErrorLogMessage = "Something went wrong when deleting entity of type {Type} from DB. \nEntity data: {Entity}";

    protected const string DeleteByIdBeginLogMessage = "Deleting entity of type {Type} with Id {Id} from DB.";
    protected const string DeleteByIdSuccessLogMessage = "Entity succesfully deleted";
    protected const string DeleteByIdErrorLogMessage = "Something went wrong when deleting entity of type {Type} from DB by Id {Id}.";

    public EntityServiceBase(IDomainContextFactory domainContextFactory, ILogger<EntityServiceBase<TEntity>> logger)
    {
        _domainContextFactory = domainContextFactory;
        _logger = logger;
    }

    protected abstract IRepository<TEntity> GetRepository(IDomainContext context);

    public virtual ICollection<TEntity> GetAll()
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(TEntity));

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);

            return repository.GetAll().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(TEntity));

            return Enumerable.Empty<TEntity>().ToList();
        }
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(TEntity));

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);

            return await repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(TEntity));

            return Enumerable.Empty<TEntity>().ToList();
        }
    }

    public virtual TEntity? GetById(int id)
    {
        try
        {
            _logger.LogInformation(GetByIdBeginLogMessage, typeof(TEntity));

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            var entity = repository.GetById(id);

            if (entity is null)
                _logger.LogWarning(GetByIdNotFoundLogMessage, typeof(TEntity), id);
            else
                _logger.LogInformation(GetByIdFoundLogMessage, entity);

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetByIdErrorLogMessage, typeof(TEntity), id);

            return null;
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation(GetByIdBeginLogMessage, typeof(TEntity));

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            var entity = await repository.GetByIdAsync(id);

            if (entity is null)
                _logger.LogWarning(GetByIdNotFoundLogMessage, typeof(TEntity), id);
            else
                _logger.LogInformation(GetByIdFoundLogMessage, entity);

            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetByIdErrorLogMessage, typeof(TEntity), id);

            return null;
        }
    }

    public virtual bool Add(TEntity entity)
    {
        try
        {
            _logger.LogInformation(AddBeginLogMessage, typeof(TEntity), entity);

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            var addedEntity = repository.Add(entity);

            context.Save();

            _logger.LogInformation(AddSuccessLogMessage, addedEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddErrorLogMessage, typeof(TEntity), entity);

            return false;
        }

        return true;
    }

    public virtual bool Update(TEntity entity)
    {
        try
        {
            _logger.LogInformation(UpdateBeginLogMessage, typeof(TEntity), entity);

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            var updatedEntity = repository.Update(entity);

            context.Save();

            _logger.LogInformation(UpdateSuccessLogMessage, updatedEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UpdateErrorLogMessage, typeof(TEntity), entity);

            return false;
        }

        return true;
    }

    public virtual bool Delete(TEntity entity)
    {
        try
        {
            _logger.LogInformation(DeleteBeginLogMessage, typeof(TEntity), entity);

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            repository.Delete(entity);

            context.Save();

            _logger.LogInformation(DeleteSuccessLogMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteErrorLogMessage, typeof(TEntity), entity);

            return false;
        }

        return true;
    }

    public virtual bool DeleteById(int id)
    {
        try
        {
            _logger.LogInformation(DeleteByIdBeginLogMessage, typeof(TEntity), id);

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);
            repository.DeleteById(id);

            context.Save();

            _logger.LogInformation(DeleteByIdSuccessLogMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteByIdErrorLogMessage, typeof(TEntity), id);

            return false;
        }

        return true;
    }
}
