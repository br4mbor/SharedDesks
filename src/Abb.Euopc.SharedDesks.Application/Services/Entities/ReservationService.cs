using Abb.Euopc.SharedDesks.Application.Common;
using Abb.Euopc.SharedDesks.Domain.Common;
using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Enums;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Context;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Responses;
using Abb.Euopc.SharedDesks.Domain.Responses.Reservation;
using Microsoft.Extensions.Logging;

namespace Abb.Euopc.SharedDesks.Application.Services.Entities;

internal sealed class ReservationService : EntityServiceBase<Reservation>, IReservationService
{
    private static readonly object m_lock = new();

    private readonly IEmailNotificationService _notificationService;

    public ReservationService(IDomainContextFactory domainContextFactory,
        ILogger<ReservationService> logger,
        IEmailNotificationService notificationService)
        : base(domainContextFactory, logger)
    {
        _notificationService = notificationService;
    }

    protected override IRepository<Reservation> GetRepository(IDomainContext context) => context.ReservationRepository;

    public override bool Add(Reservation entity)
    {
        throw new NotSupportedException();
    }

    public override bool Update(Reservation entity)
    {
        throw new NotSupportedException();
    }

    public override bool Delete(Reservation entity)
    {
        throw new NotSupportedException();
    }

    public override bool DeleteById(int id)
    {
        throw new NotSupportedException();
    }

    public async Task<bool> Cancel(Reservation reservation, string cancellingUserEmail)
    {
        try
        {
            using var context = _domainContextFactory.CreateDomainContext();

            context.ReservationRepository.Delete(reservation);
            context.Save();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteErrorLogMessage, typeof(Reservation), reservation);

            return false;
        }

        if (!string.Equals(reservation.ReservedForEmail, cancellingUserEmail, StringComparison.OrdinalIgnoreCase))
        {
            await _notificationService.SendNotificationAsync(NotificationBuilder.CreateReservationCancelMessage(reservation));
        }

        return true;
    }

    public async Task<(ICollection<Reservation>, int)> GetUsersUpcomingReservationsAsync(string userEmail, ReservationsFilterParameter parameter, int page, int pageSize)
    {
        try
        {
            _logger.LogInformation($"Getting all entities of type {typeof(Reservation)} from DB for user {userEmail}. Filter parameter: {parameter}");

            using var context = _domainContextFactory.CreateDomainContext();

            return await context.ReservationRepository.GetUsersUpcomingReservationsAsync(userEmail, parameter, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong when getting records of type {Type} from DB.", typeof(Reservation));

            return (Enumerable.Empty<Reservation>().ToList(), 0);
        }
    }

    public async Task<CreateReservationResponse> CreateReservationsAsync(IEnumerable<DateTime>? selectedDates, int deskId, string reservedForEmail, string createdByEmail)
    {
        if (selectedDates is null || !selectedDates.Any())
        {
            return new CreateReservationResponse(ResponseType.Error, "An error has occurred while creating reservation");
        }

        if (string.IsNullOrEmpty(reservedForEmail))
        {
            return new CreateReservationResponse(ResponseType.Error, "Reserved For email missing");
        }

        if (string.IsNullOrEmpty(createdByEmail))
        {
            return new CreateReservationResponse(ResponseType.Error, "Created By email missing");
        }

        using var context = _domainContextFactory.CreateDomainContext();

        var userHasReservation = await context.ReservationRepository.UserHasAnyReservationAsync(reservedForEmail, selectedDates);

        if (userHasReservation)
        {
            return new CreateReservationResponse(ResponseType.Error, $"User ({reservedForEmail}) already has any reservation");
        }

        Desk? desk;
        List<Reservation> reservations;

        try
        {
            _logger.LogInformation($"Creating reservations.");

            desk = await context.DeskRepository.GetByIdAsync(deskId);

            if (desk is null)
            {
                _logger.LogError($"Could not find desk with Id: {deskId}");

                return new CreateReservationResponse(ResponseType.Error, "Desk not found");
            }

            lock (m_lock)
            {
                var deskAvailable = context.ReservationRepository
                    .GetUpcomingReservationsByDeskId(deskId)
                    .Where(r => selectedDates.Contains(r.Date))
                    .Count() == 0;

                if (!deskAvailable)
                {
                    return new CreateReservationResponse(ResponseType.Error, "Desk already reserved");
                }

                reservations = new();

                foreach (var date in selectedDates!)
                {
                    var reservation = new Reservation()
                    {
                        DeskId = deskId,
                        CreatedByEmail = createdByEmail,
                        ReservedForEmail = reservedForEmail,
                        Date = date,
                    };

                    reservations.Add(reservation);
                }

                context.ReservationRepository.AddRange(reservations);
                context.Save();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong when creating reservations.");

            return new CreateReservationResponse(ResponseType.Error, "An error has occured");
        }

        var notifications = new List<EmailMessage>();

        if (!string.Equals(reservedForEmail, createdByEmail, StringComparison.OrdinalIgnoreCase))
        {
            foreach (var reservation in reservations)
            {
                notifications.Add(NotificationBuilder.CreateReservationMessage(reservation, desk));
            }
        }

        await _notificationService.SendNotificationsAsync(notifications);

        return new CreateReservationResponse(ResponseType.Success, "Desk reserved");
    }
}