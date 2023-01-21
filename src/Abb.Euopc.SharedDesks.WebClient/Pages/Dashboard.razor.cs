using Abb.Euopc.SharedDesks.WebClient.Components;

namespace Abb.Euopc.SharedDesks.WebClient.Pages;

public partial class Dashboard
{
    private MyReservations? _myReservations;
    private AreaOverview? _areaOverview;

    private async Task OnReservationCancelled(int? areaId)
    {
        if (_areaOverview is not null && areaId == _areaOverview.SelectedArea?.Id)
        {
            await _areaOverview.LoadAvailableDesks();
        }
    }

    private async Task OnReservationCreated()
    {
        if (_myReservations is not null)
        {
            await _myReservations.ReloadData();
        }
    }
}
