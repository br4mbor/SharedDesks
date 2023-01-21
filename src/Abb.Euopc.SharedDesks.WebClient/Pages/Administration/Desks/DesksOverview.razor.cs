using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using MudBlazor;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.Desks;

public partial class DesksOverview : OverviewBase<Desk, IDeskService, DeskDetailDialog>
{
    private string? _searchString;

    protected override string EntityName => "Desk";

    protected override async Task<TableData<Desk>> LoadDataAsync(TableState state)
    {
        var (items, totalItems) = await Service.GetAllPagedAsync(state.Page, state.PageSize, _searchString);

        return new TableData<Desk>() { TotalItems = totalItems, Items = items };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table?.ReloadServerData();
    }
}
