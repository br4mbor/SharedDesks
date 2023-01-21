namespace Abb.Euopc.SharedDesks.Domain.Requests;

public sealed class DeskFilterRequest
{
    public DateTime[] SelectedDates { get; set; } = Array.Empty<DateTime>();

    public int[]? SelectedFloors { get; set; }

    public int[]? SelectedAreaIds { get; set; }

    public int[]? SelectedItemIds { get; set; }
}
