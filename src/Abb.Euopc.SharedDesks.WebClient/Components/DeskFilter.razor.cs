using Abb.Euopc.SharedDesks.Domain.Interfaces.Services;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;
using Abb.Euopc.SharedDesks.Domain.Requests;
using Abb.Euopc.SharedDesks.WebClient.Data;
using Microsoft.AspNetCore.Components;

namespace Abb.Euopc.SharedDesks.WebClient.Components;

public partial class DeskFilter
{
    private IEnumerable<DateTime> _selectedDates = Enumerable.Empty<DateTime>();
    private readonly HashSet<TreeItemData> _treeItems = new HashSet<TreeItemData>();
    private IEnumerable<DateTime> _dates = Enumerable.Empty<DateTime>();
    private readonly TreeItemData _areas = new("Areas");
    private readonly TreeItemData _floors = new("Floors");
    private readonly TreeItemData _items = new("Equipment");
    private bool _filterLoading = false;

    [Inject]
    private IAppService AppService { get; set; } = default!;

    [Inject]
    private IAreaService AreaService { get; set; } = default!;

    [Inject]
    private IDeskItemService DeskItemService { get; set; } = default!;

    [Parameter]
    public EventCallback<DeskFilterRequest> OnFilterSubmit { get; set; }

    [Parameter]
    public EventCallback OnSelectedDatesChanged { get; set; }

    public IEnumerable<DateTime> SelectedDates
    {
        get => _selectedDates;
        set
        {
            _selectedDates = value;
            OnSelectedDatesChanged.InvokeAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadFilter();
    }

    private async Task LoadFilter()
    {
        _filterLoading = true;

        _dates = AppService.PossibleDates;

        var areas = await AreaService.GetAllAsync();
        var floors = areas.DistinctBy(a => a.Floor).OrderBy(a => a.Floor).Select(a => a.Floor).ToArray();

        foreach (var floor in floors)
        {
            _floors.AddChild(floor.ToString(), floor);
        }

        foreach (var area in areas)
        {
            _areas.AddChild(area.Name, area.Id);
        }

        var itemGroups = (await DeskItemService.GetAllActiveAsync())
            .OrderBy(i => i.Type.Name)
            .ThenBy(i => i.Name)
            .GroupBy(i => i.Type.Name);

        foreach (var group in itemGroups)
        {
            var items = new TreeItemData(group.Key);

            foreach (var item in group)
            {
                items.AddChild(item.Name, item.Id);
            }

            _items.AddChild(items);
        }

        if (_floors.HasChildren)
        {
            _treeItems.Add(_floors);
        }

        if (_areas.HasChildren)
        {
            _treeItems.Add(_areas);
        }

        if (_items.HasChildren)
        {
            _treeItems.Add(_items);
        }

        _filterLoading = false;
    }

    private async Task FilterSubmit()
    {
        await OnFilterSubmit.InvokeAsync(new DeskFilterRequest
        {
            SelectedDates = SelectedDates.ToArray(),
            SelectedFloors = _floors.Children.Where(f => f.IsChecked && f.Value is not null).Select(f => f.Value!.Value).ToArray(),
            SelectedAreaIds = _areas.Children.Where(a => a.IsChecked && a.Value is not null).Select(a => a.Value!.Value).ToArray(),
            SelectedItemIds = _items.Children.SelectMany(c => c.Children).Where(i => i.IsChecked && i.Value is not null).Select(i => i.Value!.Value).ToArray()
        });
    }

    private void FilterReset()
    {
        _selectedDates = Enumerable.Empty<DateTime>();
        _treeItems.ToList()
            .ForEach(p =>
            {
                p.Children.ToList().ForEach(c =>
                {
                    c.Children.ToList().ForEach(i => i.IsChecked = false);
                    c.IsChecked = false;
                });
                p.IsChecked = false;
            });
    }

    private static void CheckedChanged(TreeItemData item)
    {
        item.IsChecked = !item.IsChecked;

        if (item.HasChildren)
        {
            foreach (var child in item.Children)
            {
                child.IsChecked = item.IsChecked;

                if (!child.HasChildren)
                {
                    continue;
                }

                foreach (var subchild in child.Children)
                {
                    subchild.IsChecked = child.IsChecked;
                }
            }
        }

        if (item.Parent != null)
        {
            item.Parent.IsChecked = !item.Parent.Children.Any(i => !i.IsChecked);
        }
    }
}
