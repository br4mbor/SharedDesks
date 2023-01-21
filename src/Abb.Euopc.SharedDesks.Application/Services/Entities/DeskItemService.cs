using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal sealed class DeskItemService : EntityServiceBase<DeskItem>, IDeskItemService
{
    public DeskItemService(IDomainContextFactory domainContextFactory, ILogger<DeskItemService> logger)
        : base(domainContextFactory, logger)
    { }

    protected override IRepository<DeskItem> GetRepository(IDomainContext context) => context.DeskItemRepository;

    public async Task<ICollection<DeskItem>> GetAllActiveAsync()
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(DeskItem));

            using var context = _domainContextFactory.CreateDomainContext();

            return await context.DeskItemRepository.GetAllActiveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(DeskItem));

            return Enumerable.Empty<DeskItem>().ToList();
        }
    }

    public override bool Delete(DeskItem entity)
    {
        try
        {
            _logger.LogInformation(DeleteBeginLogMessage, typeof(DeskItemType));

            using var context = _domainContextFactory.CreateDomainContext();

            entity.IsDeleted = true;
            context.DeskItemRepository.Update(entity);

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

        var entity = context.DeskItemRepository.GetById(id);

        if (entity is null)
        {
            _logger.LogError(DeleteByIdErrorLogMessage, nameof(Desk), id);

            return false;
        }

        return Delete(entity);
    }
}
