using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.DeskItems;

public partial class DeskItemsOverview : OverviewBase<DeskItem, IDeskItemService, DeskItemDetailDialog>
{
    protected override string EntityName => "Desk item";

    protected override async Task<TableData<DeskItem>> LoadDataAsync(TableState state)
    {
        var data = await Service.GetAllActiveAsync();
        var totalItems = data.Count;

        return new TableData<DeskItem>() { TotalItems = totalItems, Items = data };
    }
}
