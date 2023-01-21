using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Components;

public partial class AreaOverview
{
    private IEnumerable<DateTime> _dates = Enumerable.Empty<DateTime>();
    private DateTime _selectedDate;
    private IEnumerable<Area> _areas = Enumerable.Empty<Area>();
    private IEnumerable<Desk> _desks = Enumerable.Empty<Desk>();
    private bool _loadingDesks = false;
    private bool _reservationInProgress = false;

    public Area? SelectedArea { get; set; }

    [CascadingParameter(Name = "CurrentUserEmail")]
    private string? CurrentUserEmail { get; set; }

    [Inject]
    private IAppService AppService { get; set; } = default!;

    [Inject]
    private IAreaService AreaService { get; set; } = default!;

    [Inject]
    private IDeskService DeskService { get; set; } = default!;

    [Inject]
    private IReservationService ReservationService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Parameter]
    public EventCallback ReservationCreated { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _dates = AppService.PossibleDates;
        _selectedDate = _dates.First();

        _areas = await AreaService.GetAllAsync();
    }

    private async Task SelectedAreaChanged(Area? area)
    {
        SelectedArea = area;

        if (SelectedArea is null)
        {
            _desks = Enumerable.Empty<Desk>();

            return;
        }

        await LoadAvailableDesks();
    }

    private async Task SelectedDateChanged(DateTime selectedDate)
    {
        _selectedDate = selectedDate;
        await LoadAvailableDesks();
    }

    public async Task LoadAvailableDesks()
    {
        if (SelectedArea is null)
        {
            return;
        }

        _loadingDesks = true;
        _desks = await DeskService.GetAvailableDesksAsync(_selectedDate, SelectedArea.Id);
        _loadingDesks = false;
    }

    private async Task CreateReservationAsync(Desk desk)
    {
        var confirmation = await DialogService.ShowMessageBox(new()
        {
            Title = "Create reservation",
            MarkupMessage = new MarkupString($"Do you want to create reservation of desk: <b>{desk.Label}</b> on <b>{_selectedDate:ddd, yyyy-MM-dd}</b>"),
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
        var response = await ReservationService.CreateReservationsAsync(new[] { _selectedDate }, desk.Id, CurrentUserEmail!, CurrentUserEmail!);
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

        await LoadAvailableDesks();
        await ReservationCreated.InvokeAsync();
    }
}
