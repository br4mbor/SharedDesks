using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal sealed class DeskItemTypeService : EntityServiceBase<DeskItemType>, IDeskItemTypeService
{
    public DeskItemTypeService(IDomainContextFactory domainContextFactory, ILogger<DeskItemTypeService> logger)
        : base(domainContextFactory, logger)
    { }

    protected override IRepository<DeskItemType> GetRepository(IDomainContext context) => context.DeskItemTypeRepository;

    public ICollection<DeskItemType> GetAllActive()
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(DeskItemType));

            using var context = _domainContextFactory.CreateDomainContext();

            return context.DeskItemTypeRepository.GetAllActive().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(DeskItemType));

            return Enumerable.Empty<DeskItemType>().ToList();
        }
    }

    public async Task<ICollection<DeskItemType>> GetAllActiveAsync()
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(DeskItemType));

            using var context = _domainContextFactory.CreateDomainContext();

            return await context.DeskItemTypeRepository.GetAllActiveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(DeskItemType));

            return Enumerable.Empty<DeskItemType>().ToList();
        }
    }

    public override bool Delete(DeskItemType entity)
    {
        try
        {
            _logger.LogInformation(DeleteBeginLogMessage, typeof(DeskItemType));

            using var context = _domainContextFactory.CreateDomainContext();

            entity.IsDeleted = true;
            context.DeskItemTypeRepository.Update(entity);

            context.Save();

            _logger.LogInformation(DeleteSuccessLogMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteErrorLogMessage, typeof(DeskItemType));

            return false;
        }

        return true;
    }

    public override bool DeleteById(int id)
    {
        using var context = _domainContextFactory.CreateDomainContext();

        var entity = context.DeskItemTypeRepository.GetById(id);

        if (entity is null)
        {
            _logger.LogError(DeleteByIdErrorLogMessage, nameof(Desk), id);

            return false;
        }

        return Delete(entity);
    }
}
