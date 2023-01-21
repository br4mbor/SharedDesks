using System.Text.RegularExpressions;
using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Requests;
using Abb.Euopc.SharedDesks.Domain.Responses;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages;

public partial class Reservation
{
    private IEnumerable<Desk>? _desks;
    private bool _reserveForMyself = true;
    private MudTextField<string?> _emailField = default!;
    private DeskFilterRequest? _deskFilter;
    private bool _reservationInProgress = false;

    [CascadingParameter(Name = "CurrentUserEmail")]
    private string? CurrentUserEmail { get; set; }

    [Inject]
    private IDeskService DeskService { get; set; } = default!;

    [Inject]
    private IReservationService ReservationService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    private bool ReserveForMyself
    {
        get => _reserveForMyself;
        set
        {
            _reserveForMyself = value;

            if (_reserveForMyself)
            {
                ReserveForEmail = CurrentUserEmail;
                _emailField.ResetValidation();
            }
            else
            {
                ReserveForEmail = null;
            }
        }
    }

    private string? ReserveForEmail { get; set; }

    protected override void OnParametersSet()
    {
        if (string.IsNullOrEmpty(CurrentUserEmail))
        {
            return;
        }

        ReserveForEmail = CurrentUserEmail;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!ReserveForMyself)
        {
            await _emailField.FocusAsync();
        }
    }

    private async Task GetDesksAsync(DeskFilterRequest deskFilter)
    {
        Reset();

        _deskFilter = deskFilter;
        _desks = await DeskService.GetByFilterAsync(deskFilter);
    }

    private void Reset()
    {
        _desks = null;
        _deskFilter = null;
    }

    private async Task CreateReservationAsync(int deskId)
    {
        if (_deskFilter is null)
        {
            return;
        }

        var desk = await DeskService.GetByIdAsync(deskId);

        if (desk is null)
        {
            return;
        }

        var confirmation = await DialogService.ShowMessageBox(new()
        {
            Title = "Create reservation",
            MarkupMessage = new MarkupString($"Do you want to create reservation of desk: <b>{desk.Label}</b> on <br/><b>{(string.Join("<br/>", _deskFilter.SelectedDates.Select(d => d.ToString("ddd, yyyy-MM-dd"))))}</b>"),
            YesText = "Yes, reserve!",
            NoText = "No"
        }, new()
        {
            CloseOnEscapeKey = false,
            DisableBackdropClick = true,
            FullWidth = true
        });

        if (!confirmation ?? true)
        {
            return;
        }

        StateHasChanged();

        _reservationInProgress = true;
        var response = await ReservationService.CreateReservationsAsync(_deskFilter.SelectedDates, deskId, ReserveForEmail!, CurrentUserEmail!);
        _reservationInProgress = false;

        Snackbar.Add(response.Message,
            response.Type switch
            {
                ResponseType.Error => Severity.Error,
                ResponseType.Info => Severity.Info,
                ResponseType.Normal => Severity.Normal,
                ResponseType.Success => Severity.Success,
                ResponseType.Warning => Severity.Warning,
                _ => Severity.Normal
            }
        );

        await GetDesksAsync(_deskFilter!);
    }

    private Func<string?, bool> ValidateReserveForEmail => (email) =>
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        var validator = new FluentValidation.Validators.AspNetCoreCompatibleEmailValidator<string>();
        var context = new FluentValidation.ValidationContext<string>(email);
        var regex = new Regex("@[A-Za-z]{2}\\.abb\\.com$", RegexOptions.IgnoreCase);

        return validator.IsValid(context, email) && regex.IsMatch(email);
    };
}
