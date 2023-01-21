using Abb.Euopc.SharedDesks.Application.Common;
using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Enums;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Components;

public partial class MyReservations
{
    private MudTable<Reservation>? _table;
    private MudMessageBox? _cancelReservationMessageBox;
    private ReservationsFilterParameter _filter = ReservationsFilterParameter.ReservedFor;

    [CascadingParameter(Name = "CurrentUserEmail")]
    private string? CurrentUserEmail { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IReservationService Service { get; set; } = default!;

    [Inject]
    private IEmailNotificationService NotificationService { get; set; } = default!;

    [Parameter]
    public EventCallback<int?> ReservationCancelled { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await ReloadData();
    }

    private async Task<TableData<Reservation>> LoadReservations(TableState state)
    {
        if (string.IsNullOrEmpty(CurrentUserEmail))
        {
            return new TableData<Reservation> { TotalItems = 0, Items = Enumerable.Empty<Reservation>() };
        }

        var (items, totalCount) = await Service.GetUsersUpcomingReservationsAsync(CurrentUserEmail, _filter, state.Page, state.PageSize);

        return new TableData<Reservation>() { TotalItems = totalCount, Items = items };
    }

    public async Task ReloadData()
    {
        if (_table is null)
        {
            return;
        }

        await _table.ReloadServerData();
    }

    private async Task OnFilterChanged(ReservationsFilterParameter filter)
    {
        _filter = filter;

        await ReloadData();
    }

    private async Task CancelReservation(Reservation reservation)
    {
        if (_cancelReservationMessageBox is null)
        {
            return;
        }

#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
        _cancelReservationMessageBox.MarkupMessage = new MarkupString($"Do you want to cancel reservation of desk: <b>{reservation.Desk?.Label}</b> on <b>{reservation.Date:ddd, yyyy-MM-dd}</b>");
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
        var result = await _cancelReservationMessageBox.Show(new()
        {
            CloseOnEscapeKey = false,
            DisableBackdropClick = true,
            FullWidth = true
        });

        if (!result ?? true)
        {
            return;
        }

        await Service.Cancel(reservation, CurrentUserEmail!);
        Snackbar.Add($"Reservation cancelled", Severity.Success);

        await ReloadData();
        await ReservationCancelled.InvokeAsync(reservation.Desk?.AreaId);
    }
}
