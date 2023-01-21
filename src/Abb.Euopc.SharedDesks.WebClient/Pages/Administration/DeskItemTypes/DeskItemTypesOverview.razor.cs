using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.DeskItemTypes;

public partial class DeskItemTypesOverview : OverviewBase<DeskItemType, IDeskItemTypeService, DeskItemTypeDetailDialog>
{
    protected override string EntityName => "Desk item type";

    protected override async Task<TableData<DeskItemType>> LoadDataAsync(TableState state)
    {
        var data = await Service.GetAllActiveAsync();
        var totalItems = data.Count;

        return new TableData<DeskItemType>() { TotalItems = totalItems, Items = data };
    }
}
