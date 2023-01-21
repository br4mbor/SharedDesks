using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal sealed class AreaService : EntityServiceBase<Area>, IAreaService
{
    private readonly IImageUploadService _imageUploadService;

    public AreaService(IDomainContextFactory domainContextFactory, ILogger<AreaService> logger, IImageUploadService imageUploadService)
        : base(domainContextFactory, logger)
    {
        _imageUploadService = imageUploadService;
    }

    protected override IRepository<Area> GetRepository(IDomainContext context) => context.AreaRepository;

    public async Task<bool> AddWithImageAsync(Area area, byte[]? image)
    {
        if (image?.Any() ?? false)
        {
            area.ImageUrl = await _imageUploadService.UploadImageAsync(image, nameof(Area));
        }

        return Add(area);
    }

    public async Task<bool> UpdateWithImageAsync(Area area, byte[]? image)
    {
        if (image?.Any() ?? false)
        {
            area.ImageUrl = await _imageUploadService.UploadImageAsync(image, nameof(Area));
        }

        return Update(area);
    }

    public override bool Delete(Area area)
    {
        var result = base.Delete(area);

        if (!result)
        {
            return result;
        }

        if (!string.IsNullOrEmpty(area.ImageUrl))
        {
            _imageUploadService.DeleteImageAsync(area.ImageUrl, nameof(Area));
        }

        return result;
    }
}
