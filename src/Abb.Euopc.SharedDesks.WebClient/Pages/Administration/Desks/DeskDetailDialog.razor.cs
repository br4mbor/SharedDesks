using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Validators;
using Abb.Euopc.SharedDesks.WebClient.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.Desks;

public partial class DeskDetailDialog : DetailDialogBase<Desk, IDeskService, DeskValidator>
{
    private IEnumerable<Area> _areas = Enumerable.Empty<Area>();
    private IEnumerable<DeskItem> _items = Enumerable.Empty<DeskItem>();
    private DeskItem? _itemToAdd;
    private IBrowserFile? _deskImage;

    [Inject]
    private IAreaService AreaService { get; set; } = default!;

    [Inject]
    private IDeskItemService Items { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _areas = await AreaService.GetAllAsync();
        _items = await Items.GetAllActiveAsync();
    }

    protected override async Task Save()
    {
        bool result;
        byte[]? image = null;

        if (_deskImage is not null)
        {
            image = await ImageHelper.ConvertImageAsync(_deskImage);
        }

        if (_model.Id == 0)
        {
            result = await Service.AddDeskWithImageAsync(_model, image);
        }
        else
        {
            result = await Service.UpdateDeskWithImageAsync(_model, image);
        }

        Dialog.Close(DialogResult.Ok(result));
    }

    private void AddItem(IEnumerable<DeskItem> items)
    {
        var item = items?.FirstOrDefault();

        if (item is null)
        {
            return;
        }

        _model.DeskItemsToDesks.Add(new DeskItemToDesk
        {
            DeskId = _model.Id,
            DeskItem = item,
            DeskItemId = item.Id
        });

        _itemToAdd = null;
    }

    private void RemoveItem(MudChip chip)
    {
        if (chip.Value is not DeskItemToDesk item)
        {
            return;
        }

        _model.DeskItemsToDesks.Remove(item);
    }
}
