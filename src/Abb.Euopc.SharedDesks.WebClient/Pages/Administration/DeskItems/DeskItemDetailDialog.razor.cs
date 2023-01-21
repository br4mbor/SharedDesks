using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Validators;
using Microsoft.AspNetCore.Components;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.DeskItems;

public partial class DeskItemDetailDialog : DetailDialogBase<DeskItem, IDeskItemService, DeskItemValidator>
{
    private IEnumerable<DeskItemType> _types = Enumerable.Empty<DeskItemType>();

    [Inject]
    private IDeskItemTypeService TypeService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _types = await TypeService.GetAllActiveAsync();

        if (_model.TypeId == 0)
        {
            _model.Type = _types.FirstOrDefault()!;
        }
    }
}
