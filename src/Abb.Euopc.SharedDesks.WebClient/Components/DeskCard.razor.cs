using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.WebClient.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Components;

public partial class DeskCard
{
    [Parameter]
    public Desk? Desk { get; set; }

    [Parameter]
    public bool ReserveButtonDisabled { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> ReserveButtonClick { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    private void ShowDetail()
    {
        if (Desk is null)
        {
            return;
        }

        DialogService.Show<DeskDetailDialog>(Desk.Label,
            new() { ["DeskId"] = Desk.Id },
            new()
            {
                CloseButton = true,
                CloseOnEscapeKey = false,
                DisableBackdropClick = false,
                FullWidth = true
            });
    }
}
