using Abb.Euopc.SharedDesks.Application.Common;
using Abb.Euopc.SharedDesks.Domain.Common;
using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Requests;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal sealed class DeskService : EntityServiceBase<Desk>, IDeskService
{
    private readonly IImageUploadService _imageUploadService;
    private readonly IEmailNotificationService _notificationService;

    public DeskService(IDomainContextFactory domainContextFactory,
        ILogger<DeskService> logger,
        IImageUploadService imageUploadService,
        IEmailNotificationService notificationService)
        : base(domainContextFactory, logger)
    {
        _imageUploadService = imageUploadService;
        _notificationService = notificationService;
    }

    public async Task<(ICollection<Desk>, int)> GetAllPagedAsync(int page, int pageSize, string? searchString = null)
    {
        try
        {
            _logger.LogInformation(GetAllBeginLogMessage, typeof(Desk));

            using var context = _domainContextFactory.CreateDomainContext();

            var repository = GetRepository(context);

            return await repository.GetAllPagedAsync(page, pageSize, searchString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, GetAllErrorLogMessage, typeof(Desk));

            return (Enumerable.Empty<Desk>().ToList(), 0);
        }
    }

    protected override IDeskRepository GetRepository(IDomainContext context) => context.DeskRepository;

    public override bool Update(Desk desk)
    {
        var reservationGroups = Enumerable.Empty<(string ReservedForEmail, DateTime[] Dates)>();

        try
        {
            _logger.LogInformation(UpdateBeginLogMessage, typeof(Desk), desk);

            using var context = _domainContextFactory.CreateDomainContext();

            reservationGroups = context.ReservationRepository
                .GetUpcomingReservationsByDeskId(desk.Id)
                .GroupBy(r => r.ReservedForEmail, r => r.Date, (key, g) => (key, g.Select(r => r.Date).ToArray()))
                .ToList();

            var updatedDesk = context.DeskRepository.Update(desk);
            context.Save();

            _logger.LogInformation(UpdateSuccessLogMessage, updatedDesk);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UpdateErrorLogMessage, typeof(Desk), desk);

            return false;
        }

        var notifications = new List<EmailMessage>();

        foreach (var (reservedForEmail, dates) in reservationGroups)
        {
            notifications.Add(NotificationBuilder.CreateDeskUpdateMessage(desk, reservedForEmail, dates));
        }

        _notificationService.SendNotificationsAsync(notifications);

        return true;
    }

    public override bool Delete(Desk desk)
    {
        IEnumerable<Reservation> reservations;

        try
        {
            using var context = _domainContextFactory.CreateDomainContext();

            reservations = context.ReservationRepository
                .GetUpcomingReservationsByDeskId(desk.Id)
                .ToList();

            context.DeskRepository.Delete(desk);
            context.Save();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteErrorLogMessage, typeof(Desk), desk);

            return false;
        }

        if (!string.IsNullOrEmpty(desk.ImageUrl))
        {
            _imageUploadService.DeleteImageAsync(desk.ImageUrl, nameof(Desk));
        }

        var notifications = new List<EmailMessage>();

        foreach (var reservation in reservations)
        {
            reservation.Desk = desk;
            notifications.Add(NotificationBuilder.CreateReservationCancelMessage(reservation));
        }

        _notificationService.SendNotificationsAsync(notifications);

        return true;
    }

    public override bool DeleteById(int id)
    {
        using var context = _domainContextFactory.CreateDomainContext();

        var desk = context.DeskRepository.GetById(id);

        if (desk is null)
        {
            _logger.LogError(DeleteByIdErrorLogMessage, nameof(Desk), id);

            return false;
        }

        return Delete(desk);
    }

    public async Task<bool> AddDeskWithImageAsync(Desk desk, byte[]? image)
    {
        if (image?.Any() ?? false)
        {
            desk.ImageUrl = await _imageUploadService.UploadImageAsync(image, nameof(Desk));
        }

        return Add(desk);
    }

    public async Task<bool> UpdateDeskWithImageAsync(Desk desk, byte[]? image)
    {
        if (image?.Any() ?? false)
        {
            desk.ImageUrl = await _imageUploadService.UploadImageAsync(image, nameof(Desk));
        }

        return Update(desk);
    }

    public async Task<ICollection<Desk>> GetByFilterAsync(DeskFilterRequest deskFilter)
    {
        try
        {
            using var context = _domainContextFactory.CreateDomainContext();

            var desks = await context.DeskRepository.GetByFilterAsync(deskFilter);

            return desks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Desk}", typeof(Desk));

            return Enumerable.Empty<Desk>().ToList();
        }
    }

    public async Task<ICollection<Desk>> GetAvailableDesksAsync(DateTime date, int areaId)
    {
        try
        {
            using var context = _domainContextFactory.CreateDomainContext();

            var desks = await context.DeskRepository.GetAvailableDesksAsync(date, areaId);

            return desks;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Desk}", typeof(Desk));

            return Enumerable.Empty<Desk>().ToList();
        }
    }

    public async Task<bool> IsUniqueLabelAsync(int deskId, string label)
    {
        try
        {
            using var context = _domainContextFactory.CreateDomainContext();

            return await context.DeskRepository.IsUniqueLabelAsync(deskId, label);
        }
        catch
        {
            return false;
        }
    }
}
