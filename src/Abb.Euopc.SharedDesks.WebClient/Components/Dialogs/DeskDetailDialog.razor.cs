using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Microsoft.AspNetCore.Components;

namespace Abb.Euopc.SharedDesks.WebClient.Components.Dialogs;

public partial class DeskDetailDialog
{
    private Desk? _desk;

    [Parameter]
    public int DeskId { get; set; }

    [Inject]
    private IDeskService DeskService { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        _desk = await DeskService.GetByIdAsync(DeskId);
    }
}
