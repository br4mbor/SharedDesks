using Abb.Euopc.SharedDesks.Domain.Entities;
using Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Entities;

namespace Abb.Euopc.SharedDesks.WebClient.Pages.Administration.Areas;

public partial class AreasOverview : OverviewBase<Area, IAreaService, AreaDetailDialog>
{
    protected override string EntityName => "Area";
}
